export class ApiError<TData = unknown> extends Error {
  readonly status: number;
  readonly data: TData;

  constructor(message: string, status: number, data: TData) {
    super(message);

    this.name = "ApiError";
    this.status = status;
    this.data = data;
  }
}
