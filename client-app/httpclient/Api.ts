/* eslint-disable */
/* tslint:disable */

/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface AuthenticationResponseDto {
    token?: string | null;
}

export interface BooleanFAnonymousType1 {
    success?: boolean;
}

export interface CreateTagRequestDto {
    name?: string | null;
}

export interface CreateTodoRequestDto {
    /** @minLength 1 */
    title?: string | null;
    description?: string | null;
    /** @format date-time */
    dueDate?: string;
    /** @format int32 */
    priority?: number;
    tags?: Tag[] | null;
}

export interface RegisterDto {
    username?: string | null;
    password?: string | null;
}

export interface SignInDto {
    username?: string | null;
    password?: string | null;
}

export interface Tag {
    /** @format int32 */
    id?: number;
    name?: string | null;
    /** @format int32 */
    userId?: number;
}

export interface TodoWithTags {
    /** @format int32 */
    id?: number;
    title?: string | null;
    description?: string | null;
    /** @format date-time */
    dueDate?: string;
    isCompleted?: boolean;
    /** @format date-time */
    createdAt?: string;
    /** @format int32 */
    priority?: number;
    /** @format int32 */
    userId?: number;
    tags?: Tag[] | null;
}

export interface UpdateTodoRequestDto {
    /** @format int32 */
    id?: number;
    title?: string | null;
    description?: string | null;
    /** @format date-time */
    dueDate?: string;
    isCompleted?: boolean;
    /** @format int32 */
    priority?: number;
}

import type {AxiosInstance, AxiosRequestConfig, AxiosResponse, HeadersDefaults, ResponseType} from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
    /** set parameter to `true` for call `securityWorker` for this request */
    secure?: boolean;
    /** request path */
    path: string;
    /** content type of request body */
    type?: ContentType;
    /** query params */
    query?: QueryParamsType;
    /** format of response (i.e. response.json() -> format: "json") */
    format?: ResponseType;
    /** request body */
    body?: unknown;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
    securityWorker?: (
        securityData: SecurityDataType | null,
    ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
    secure?: boolean;
    format?: ResponseType;
}

export enum ContentType {
    Json = "application/json",
    FormData = "multipart/form-data",
    UrlEncoded = "application/x-www-form-urlencoded",
    Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
    public instance: AxiosInstance;
    private securityData: SecurityDataType | null = null;
    private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
    private secure?: boolean;
    private format?: ResponseType;

    constructor({securityWorker, secure, format, ...axiosConfig}: ApiConfig<SecurityDataType> = {}) {
        this.instance = axios.create({...axiosConfig, baseURL: axiosConfig.baseURL || ""});
        this.secure = secure;
        this.format = format;
        this.securityWorker = securityWorker;
    }

    public setSecurityData = (data: SecurityDataType | null) => {
        this.securityData = data;
    };

    public request = async <T = any, _E = any>({
                                                   secure,
                                                   path,
                                                   type,
                                                   query,
                                                   format,
                                                   body,
                                                   ...params
                                               }: FullRequestParams): Promise<AxiosResponse<T>> => {
        const secureParams =
            ((typeof secure === "boolean" ? secure : this.secure) &&
                this.securityWorker &&
                (await this.securityWorker(this.securityData))) ||
            {};
        const requestParams = this.mergeRequestParams(params, secureParams);
        const responseFormat = format || this.format || undefined;

        if (type === ContentType.FormData && body && body !== null && typeof body === "object") {
            body = this.createFormData(body as Record<string, unknown>);
        }

        if (type === ContentType.Text && body && body !== null && typeof body !== "string") {
            body = JSON.stringify(body);
        }

        return this.instance.request({
            ...requestParams,
            headers: {
                ...(requestParams.headers || {}),
                ...(type && type !== ContentType.FormData ? {"Content-Type": type} : {}),
            },
            params: query,
            responseType: responseFormat,
            data: body,
            url: path,
        });
    };

    protected mergeRequestParams(params1: AxiosRequestConfig, params2?: AxiosRequestConfig): AxiosRequestConfig {
        const method = params1.method || (params2 && params2.method);

        return {
            ...this.instance.defaults,
            ...params1,
            ...(params2 || {}),
            headers: {
                ...((method && this.instance.defaults.headers[method.toLowerCase() as keyof HeadersDefaults]) || {}),
                ...(params1.headers || {}),
                ...((params2 && params2.headers) || {}),
            },
        };
    }

    protected stringifyFormItem(formItem: unknown) {
        if (typeof formItem === "object" && formItem !== null) {
            return JSON.stringify(formItem);
        } else {
            return `${formItem}`;
        }
    }

    protected createFormData(input: Record<string, unknown>): FormData {
        return Object.keys(input || {}).reduce((formData, key) => {
            const property = input[key];
            const propertyContent: any[] = property instanceof Array ? property : [property];

            for (const formItem of propertyContent) {
                const isFileType = formItem instanceof Blob || formItem instanceof File;
                formData.append(key, isFileType ? formItem : this.stringifyFormItem(formItem));
            }

            return formData;
        }, new FormData());
    }
}

/**
 * @title My API
 * @version v1
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
    api = {
        /**
         * No description
         *
         * @tags api
         * @name RegisterCreate
         * @request POST:/api/register
         * @secure
         */
        registerCreate: (data: RegisterDto, params: RequestParams = {}) =>
            this.request<AuthenticationResponseDto, any>({
                path: `/api/register`,
                method: "POST",
                body: data,
                secure: true,
                type: ContentType.Json,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name SigninCreate
         * @request POST:/api/signin
         * @secure
         */
        signinCreate: (data: SignInDto, params: RequestParams = {}) =>
            this.request<AuthenticationResponseDto, any>({
                path: `/api/signin`,
                method: "POST",
                body: data,
                secure: true,
                type: ContentType.Json,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name TodosCreate
         * @request POST:/api/todos
         * @secure
         */
        todosCreate: (data: CreateTodoRequestDto, params: RequestParams = {}) =>
            this.request<TodoWithTags, any>({
                path: `/api/todos`,
                method: "POST",
                body: data,
                secure: true,
                type: ContentType.Json,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name TodosList
         * @request GET:/api/todos
         * @secure
         */
        todosList: (
            query: {
                serializedTagArray: string;
                orderBy: string;
                direction: string;
                showCompleted: boolean;
                /**
                 * @format int32
                 * @default 50
                 */
                limit?: number;
            },
            params: RequestParams = {},
        ) =>
            this.request<TodoWithTags[], any>({
                path: `/api/todos`,
                method: "GET",
                query: query,
                secure: true,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name TodoDelete
         * @request DELETE:/api/todo/{id}
         * @secure
         */
        todoDelete: (id: number, params: RequestParams = {}) =>
            this.request<BooleanFAnonymousType1, any>({
                path: `/api/todo/${id}`,
                method: "DELETE",
                secure: true,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name TodosUpdate
         * @request PUT:/api/todos/{id}
         * @secure
         */
        todosUpdate: (id: string, data: UpdateTodoRequestDto, params: RequestParams = {}) =>
            this.request<TodoWithTags, any>({
                path: `/api/todos/${id}`,
                method: "PUT",
                body: data,
                secure: true,
                type: ContentType.Json,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name TagsAddToTodoCreate
         * @request POST:/api/tags/{tagId}/addToTodo/{todoId}
         * @secure
         */
        tagsAddToTodoCreate: (tagId: number, todoId: number, params: RequestParams = {}) =>
            this.request<BooleanFAnonymousType1, any>({
                path: `/api/tags/${tagId}/addToTodo/${todoId}`,
                method: "POST",
                secure: true,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name TagsCreate
         * @request POST:/api/tags
         * @secure
         */
        tagsCreate: (data: CreateTagRequestDto, params: RequestParams = {}) =>
            this.request<Tag, any>({
                path: `/api/tags`,
                method: "POST",
                body: data,
                secure: true,
                type: ContentType.Json,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name TagsList
         * @request GET:/api/tags
         * @secure
         */
        tagsList: (params: RequestParams = {}) =>
            this.request<Tag[], any>({
                path: `/api/tags`,
                method: "GET",
                secure: true,
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name DeleteApi
         * @request DELETE:/api/tag/{id}
         * @secure
         */
        deleteApi: (id: number, params: RequestParams = {}) =>
            this.request<void, any>({
                path: `/api/tag/${id}`,
                method: "DELETE",
                secure: true,
                ...params,
            }),

        /**
         * No description
         *
         * @tags api
         * @name TagsRemoveFromTodoDelete
         * @request DELETE:/api/tags/{tagId}/removeFromTodo/{todoId}
         * @secure
         */
        tagsRemoveFromTodoDelete: (tagId: number, todoId: number, params: RequestParams = {}) =>
            this.request<BooleanFAnonymousType1, any>({
                path: `/api/tags/${tagId}/removeFromTodo/${todoId}`,
                method: "DELETE",
                secure: true,
                format: "json",
                ...params,
            }),
    };
}
