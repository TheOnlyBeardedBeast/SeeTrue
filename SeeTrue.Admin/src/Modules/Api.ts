export interface UserRequest {
  userId: string;
}

export interface UserResponse<T = any> {
  instanceID: string;
  id: string;
  aud: string;
  role: string;
  email: string;
  language?: any;
  confirmedAt: Date;
  invitedAt?: any;
  recoverySentAt?: any;
  emailChange?: any;
  emailChangeSentAt?: any;
  lastSignInAt: Date;
  appMetaData: T;
  userMetaData?: any;
  isSuperAdmin?: any;
  createdAt: Date;
  updatedAt: Date;
}

export interface UsersResponse<T = any> {
  page: number;
  perPage: number;
  itemCount: number;
  items: UserResponse<T>[];
}

export class Api {
  protected path: string = "admin/users";
  public readonly host: string;
  public readonly apiKey?: string;

  constructor(host: string, apiKey?: string) {
    this.host = host;
    this.apiKey = apiKey;
  }

  public async getUser(id: string, accessToken?: string) {
    const auth = this.getAuthHeader(accessToken);

    if (!id) {
      throw new Error("Missing required parameter");
    }

    const response = await fetch(`${this.host}/${this.path}/${id}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
      body: JSON.stringify({
        userId: id,
      } as UserRequest),
    });

    const result = await response.json();

    return result as UserResponse;
  }
  public async getUsers(
    page: number = 1,
    perPage: number = 20,
    accessToken?: string
  ) {
    const auth = this.getAuthHeader(accessToken);

    const params = new URLSearchParams();
    params.append("page", page.toString());
    params.append("perPage", perPage.toString());

    const response = await fetch(
      `${this.host}/${this.path}?${params.toString()}`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          ...auth,
        } as any,
      }
    );

    const result = await response.json();

    return result as UsersResponse;
  }

  createUser(accessToken?: string) {
    if (!this.apiKey || !accessToken) {
      throw new Error("Authorization not configured");
    }
  }
  deleteUser(id: string, accessToken?: string) {
    if (!this.apiKey || !accessToken) {
      throw new Error("Authorization not configured");
    }
  }

  protected getAuthHeader(accessToken?: string) {
    if (!this.apiKey && !accessToken) {
      throw new Error("Authorization not configured");
    }

    return this.apiKey
      ? { "X-API-KEY": this.apiKey }
      : { Authorization: `Bearer ${accessToken}` };
  }
}