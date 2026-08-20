import { FetchClient } from "../api/FetchClient";

export interface AnimeCharacter {
  id: number;
  name: {
    full: string;
    native: string | null;
  };
  image: {
    large: string;
    medium: string;
  };
  description: string | null;
}

interface GraphQLResponse<T> {
  data: T;
  errors?: Array<{
    message: string;
  }>;
}

interface CharacterQueryResponse {
  Character: AnimeCharacter;
}

interface CharactersQueryResponse {
  Page: {
    characters: AnimeCharacter[];
  };
}

class AnimeCharacterService {
  private readonly client: FetchClient;

  /**
   * Creates a service connected to AniList's GraphQL API.
   *
   * Example:
   * const animeService = new AnimeCharacterService();
   */
  constructor() {
    this.client = new FetchClient("https://graphql.anilist.co");
  }

  /**
   * Retrieves one anime character by its AniList ID.
   *
   * @param id The unique AniList character ID.
   * @returns The matching character's profile, images, and description.
   * @throws Error when AniList returns a GraphQL error or the request fails.
   *
   * Example:
   * const character = await animeService.getCharacter(125);
   * console.log(character.name.full);
   */
  async getCharacter(id: number): Promise<AnimeCharacter> {
    const query = `
      query ($id: Int) {
        Character(id: $id) {
          id
          name {
            full
            native
          }
          image {
            large
            medium
          }
          description
        }
      }
    `;

    const response = await this.client.post<
      GraphQLResponse<CharacterQueryResponse>
    >("", {
      query,
      variables: {
        id,
      },
    });

    if (response.errors?.length) {
      throw new Error(response.errors[0].message);
    }

    return response.data.Character;
  }

  /**
   * Searches AniList characters by name with pagination.
   *
   * @param search The character name or partial name to search for.
   * @param page The one-based page number to retrieve.
   * @param perPage The number of characters to return per page.
   * @returns An array of matching anime characters.
   * @throws Error when AniList returns a GraphQL error or the request fails.
   *
   * Example:
   * const characters = await animeService.searchCharacters("Sailor", 1, 10);
   * characters.forEach((character) => console.log(character.name.full));
   */
  async searchCharacters(
    search: string,
    page: number = 1,
    perPage: number = 20,
  ): Promise<AnimeCharacter[]> {
    const query = `
      query (
        $search: String
        $page: Int
        $perPage: Int
      ) {
        Page(
          page: $page
          perPage: $perPage
        ) {
          characters(search: $search) {
            id
            name {
              full
              native
            }
            image {
              large
              medium
            }
            description
          }
        }
      }
    `;

    const response = await this.client.post<
      GraphQLResponse<CharactersQueryResponse>
    >("", {
      query,
      variables: {
        search,
        page,
        perPage,
      },
    });

    if (response.errors?.length) {
      throw new Error(response.errors[0].message);
    }

    return response.data.Page.characters;
  }
}

export const animeService = new AnimeCharacterService();
