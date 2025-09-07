type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE';

export interface ApiRequest<T = void> {
    endpoint: string,
    method: HttpMethod,
    body: unknown | null
}
