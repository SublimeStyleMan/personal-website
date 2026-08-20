import { ApiError } from "./ApiError";

export type QueryParams = Record<
  string,
  string | number | boolean | null | undefined
>;

export type RequestBody = BodyInit | Record<string, unknown> | unknown[];

export interface RequestOptions {
  method?: string;
  headers?: Record<string, string>;
  body?: RequestBody;
  params?: QueryParams;
  signal?: AbortSignal;
}

export class FetchClient {
  private readonly baseUrl: string;

  /** Creates a client that resolves relative endpoints against the supplied base URL. */
  constructor(baseUrl = "") {
    this.baseUrl = baseUrl;
  }

  /**
   * Sends an HTTP request and returns the response parsed as the requested type.
   * Network failures become regular Errors, while non-2xx responses become ApiErrors.
   */
  async request<TResponse = unknown>(
    endpoint: string,
    { method = "GET", headers = {}, body, params, signal }: RequestOptions = {},
  ): Promise<TResponse> {
    const url = this.buildUrl(endpoint, params);
    const requestHeaders: Record<string, string> = {
      Accept: "application/json",
      ...headers,
    };
    let requestBody: BodyInit | undefined;

    // Keep FormData and other native request bodies intact; serialize objects as JSON.
    if (body instanceof FormData || typeof body === "string") {
      requestBody = body;
    } else if (body !== undefined) {
      requestHeaders["Content-Type"] = "application/json";
      requestBody = JSON.stringify(body);
    }

    let response: Response;
    try {
      response = await fetch(url, {
        method,
        headers: requestHeaders,
        body: requestBody,
        signal,
      });
    } catch (error: unknown) {
      // Convert transport failures into one consistent error for callers.
      const message = error instanceof Error ? error.message : String(error);
      throw new Error(`Network request failed: ${message}`);
    }

    const data = await this.parseResponse<TResponse>(response);
    if (!response.ok) {
      const errorMessage = this.getErrorMessage(data, response.status);
      throw new ApiError(errorMessage, response.status, data);
    }

    return data;
  }

  /**
   * Parses a response as JSON when its content type is JSON; otherwise returns its text.
   * Empty 204 responses resolve to undefined for the caller's response type.
   */
  async parseResponse<TResponse>(response: Response): Promise<TResponse> {
    // Empty responses cannot be parsed as JSON, so return undefined for 204 responses.
    if (response.status === 204) {
      return undefined as TResponse;
    }

    const contentType = response.headers.get("content-type");
    if (contentType?.includes("application/json")) {
      return (await response.json()) as TResponse;
    }

    return (await response.text()) as TResponse;
  }

  /** Builds an absolute URL and appends defined query parameters. */
  buildUrl(endpoint: string, params?: QueryParams): string {
    const url = new URL(endpoint, this.baseUrl);
    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        if (value !== undefined && value !== null) {
          url.searchParams.append(key, String(value));
        }
      });
    }
    return url.toString();
  }

  /** Sends a GET request with optional query parameters. */
  get<TResponse = unknown>(
    endpoint: string,
    params?: QueryParams,
    options: Omit<RequestOptions, "method" | "body" | "params"> = {},
  ): Promise<TResponse> {
    return this.request<TResponse>(endpoint, {
      ...options,
      method: "GET",
      params,
    });
  }

  /** Sends a POST request with an optional request body. */
  post<TResponse = unknown>(
    endpoint: string,
    body?: RequestBody,
    options: Omit<RequestOptions, "method" | "body"> = {},
  ): Promise<TResponse> {
    return this.request<TResponse>(endpoint, {
      ...options,
      method: "POST",
      body,
    });
  }

  /** Sends a PUT request with an optional request body. */
  put<TResponse = unknown>(
    endpoint: string,
    body?: RequestBody,
    options: Omit<RequestOptions, "method" | "body"> = {},
  ): Promise<TResponse> {
    return this.request<TResponse>(endpoint, {
      ...options,
      method: "PUT",
      body,
    });
  }

  /** Sends a PATCH request with an optional request body. */
  patch<TResponse = unknown>(
    endpoint: string,
    body?: RequestBody,
    options: Omit<RequestOptions, "method" | "body"> = {},
  ): Promise<TResponse> {
    return this.request<TResponse>(endpoint, {
      ...options,
      method: "PATCH",
      body,
    });
  }

  /** Sends a DELETE request. */
  delete<TResponse = unknown>(
    endpoint: string,
    options: Omit<RequestOptions, "method" | "body" | "params"> = {},
  ): Promise<TResponse> {
    return this.request<TResponse>(endpoint, { ...options, method: "DELETE" });
  }

  /** Extracts a server-provided message or creates a status-based fallback. */
  private getErrorMessage<TResponse>(data: TResponse, status: number): string {
    if (typeof data === "object" && data !== null && "message" in data) {
      const message = data.message;
      if (typeof message === "string") {
        return message;
      }
    }
    return `Request failed with status ${status}`;
  }
}
