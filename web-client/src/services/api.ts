export async function fetchLogin(username: string, password: string): Promise<void> {
    return fetchEndpointNoResult("https://localhost:7262/login", "POST", { Username: username, Password: password });
}

export async function fetchHome(): Promise<string> {
    var response = await fetchEndpoint("https://localhost:7262/", "GET", null);
    return response.text();
}

export async function fetchRegister(username: string, password: string, email: string) {
    return fetchEndpointNoResult("https://localhost:7262/register", "POST", { Username: username, Password: password, Email: email });
}


async function fetchEndpointWithResult<TResult>(path: string, method: "GET" | "POST" | "PUT" | "DELETE", body: any): Promise<TResult> {
    const res = await fetchEndpoint(path, method, body);
    return (await res.json()) as TResult;
}

async function fetchEndpointNoResult(path: string, method: "GET" | "POST" | "PUT" | "DELETE", body: any): Promise<void> {
    await fetchEndpoint(path, method, body);
}

async function fetchEndpoint(path: string, method: "GET" | "POST" | "PUT" | "DELETE", body: any): Promise<Response> {
    const res = await fetch(path, {
        credentials: "include",
        method: method,
        headers: {
            "Content-Type": "application/json",
        },
        body: method !== "GET" ? JSON.stringify(body) : undefined,
    });

    if (res.status === 400) {
        const json = await res.json();
        throw new Error(json["message"]);
    }

    if(res.status === 401) {
        throw new Error("Unauthorized");
    }
    
    if (!res.ok) {
        throw new Error(`Error ${res.status}`);
    }

    return res;
}