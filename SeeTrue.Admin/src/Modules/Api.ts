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

export interface PaginationResponse<T = any> {
  page: number;
  perPage: number;
  itemCount: number;
  items: T[];
}

export enum NotificationType {
  Confirmation,
  EmailChange,
  InviteUser,
  MagicLink,
  Recovery,
}

export interface UserRequest {
  id?: string;
  role?: string;
  audience?: string;
  confirm?: boolean | null;
  password?: string;
  email?: string;
  appMetaData?: { [key: string]: any };
  userMetaData?: { [key: string]: any };
  language?: string;
}

export interface MailRequest {
  id?: string;
  type: NotificationType;
  language: string;
  template: string;
  content: string;
  audience: string;
  subject: string;
}
export interface MailResponse extends MailRequest {
  id: string;
}

export interface IAdminSettings {
  roles: string[];
  languages: string[];
  audiences: string[];
  emailTypes: { [key: string]: number };
}

export class Api {
  protected path: string = "admin/users";
  public host?: string;
  public apiKey?: string;

  constructor(host?: string, apiKey?: string) {
    this.host = host;
    this.apiKey = apiKey;
  }

  public async authorize(config: { accessToken?: string; apiKey?: string }) {
    const auth = this.getAuthHeader(config.accessToken, config.apiKey);

    const response = await fetch(`${this.host}/Admin/authorize`, {
      method: "POST",
      headers: {
        ...auth,
      } as any,
    });

    if (response.status === 204 && config.apiKey) {
      // this.apiKey = config.apiKey;
      return;
    }

    throw new Error("Unauthorized");
  }

  public async getUser(id: string, accessToken?: string) {
    const auth = this.getAuthHeader(accessToken);

    const response = await fetch(`${this.host}/${this.path}/${id}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
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

    return result as PaginationResponse<UserResponse>;
  }

  public async getMails(
    page: number = 1,
    perPage: number = 20,
    accessToken?: string
  ) {
    const auth = this.getAuthHeader(accessToken);

    const params = new URLSearchParams();
    params.append("page", page.toString());
    params.append("perPage", perPage.toString());

    const response = await fetch(
      `${this.host}/admin/mails?${params.toString()}`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          ...auth,
        } as any,
      }
    );

    const result = await response.json();

    return result as PaginationResponse<MailResponse>;
  }

  public async getMail(id: string, accessToken?: string) {
    const auth = this.getAuthHeader(accessToken);

    const response = await fetch(`${this.host}/admin/mails/${id}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
    });

    if (!response.ok) {
      throw new Error(response.status.toString());
    }

    const result = await response.json();

    return result as MailResponse;
  }

  public async createUser(user: UserRequest, accessToken?: string) {
    const auth = this.getAuthHeader(accessToken);

    const result = await fetch(`${this.host}/${this.path}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
      body: JSON.stringify(user),
    });

    if (result.status !== 200) {
      throw new Error("Failed to create user");
    }
  }

  public async updateUser(user: UserRequest, accessToken?: string) {
    const auth = this.getAuthHeader(accessToken);

    const result = await fetch(`${this.host}/${this.path}/${user.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
      body: JSON.stringify(user),
    });

    if (result.status !== 200) {
      throw new Error("Failed to create user");
    }
  }

  public async deleteUser(id: string, accessToken?: string) {
    const auth = this.getAuthHeader(accessToken);

    await fetch(`${this.host}/${this.path}/${id.toString()}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
    });
  }

  public async settings(accessToken?: string): Promise<IAdminSettings> {
    const auth = this.getAuthHeader(accessToken);

    const response = await fetch(`${this.host}/admin/settings`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
    });

    return response.json();
  }

  public async createMail(
    data: MailRequest,
    accessToken?: string
  ): Promise<MailResponse> {
    const auth = this.getAuthHeader(accessToken);

    const response = await fetch(`${this.host}/admin/mails`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
      body: JSON.stringify(data),
    });

    return response.json();
  }

  public async updateMail(
    data: MailRequest,
    accessToken?: string
  ): Promise<MailResponse> {
    const auth = this.getAuthHeader(accessToken);

    const response = await fetch(`${this.host}/admin/mails`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
      body: JSON.stringify(data),
    });

    return response.json();
  }

  public async deleteMail(id: string, accessToken?: string) {
    const auth = this.getAuthHeader(accessToken);

    const response = await fetch(`${this.host}/admin/mails/${id}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        ...auth,
      } as any,
    });

    return response.json();
  }

  protected getAuthHeader(accessToken?: string, apikey?: string) {
    if (!this.apiKey && !accessToken && !apikey) {
      throw new Error("Authorization not configured");
    }

    return accessToken
      ? { Authorization: `Bearer ${accessToken}` }
      : { "X-API-KEY": this.apiKey ?? apikey };
  }
}
