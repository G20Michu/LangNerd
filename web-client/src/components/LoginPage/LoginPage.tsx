import React, { useState } from "react";
import { execute } from "../../services/api";
import LoginRequest from "../../apiRequests/login";

function LoginPage() {
    const [loginName, setLoginName] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = async () => {
        var a = await execute(new LoginRequest(loginName, password));
    }

    return (
        <div>
            <h1>Login</h1>
            <input type="text" placeholder="Login Name" value={loginName} onChange={(e) => setLoginName(e.target.value)} />
            <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
            <button onClick={handleLogin}>Login</button>
        </div>
    )
}

export default LoginPage;