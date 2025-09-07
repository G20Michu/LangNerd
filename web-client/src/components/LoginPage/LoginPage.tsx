import React, { useState } from "react";
import { fetchLogin } from "../../services/api";
import { useNavigate } from "react-router";

function LoginPage() {
    const [loginName, setLoginName] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleLogin = async () => {
        await fetchLogin(loginName, password);

        // redirect to home page
        navigate('/');
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