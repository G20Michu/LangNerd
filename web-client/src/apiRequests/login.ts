import { ApiRequest } from "../services/apiRequest";

class LoginRequest implements ApiRequest{

    constructor(loginName: string, password: string) {
        this.body = {
            loginName: loginName,
            password: password
        }
    }

    endpoint: string = "/api/login";
    method: "GET" | "POST" | "PUT" | "DELETE" = "POST";
    body;
}

export default LoginRequest;