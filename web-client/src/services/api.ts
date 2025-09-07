import { ApiRequest } from "./apiRequest";

export async function execute<T = void>(req: ApiRequest<T>): Promise<T> {
    const res = await fetch(req.endpoint, {
        method: req.method,
        body: req.body == null ? undefined : JSON.stringify(req.body)
    });

    if(!res.ok) {
        throw new Error('Http request was not successfull, TODO: exact error message');
    }

    // if request is not void parse result to T and return it
    type IsVoid = T extends void ? true : false;
    var isVoid: IsVoid = null as any;
    if (isVoid) {
        const data = await res.json();
        return data as T;
    }

    return void 0 as T;
}