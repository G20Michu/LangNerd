import { useState } from "react";
import { fetchRegister } from "../../services/api";
import { useNavigate } from "react-router";

function RegisterPage(){
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const navigate = useNavigate();
    
    const handleRegister = async () => {
        await fetchRegister(username, password, email);
        navigate("/login");
    }
    return (
        <div>
            <h1>Register</h1>
            <input type="text" placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} />
            <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
            <input type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} />
            <button onClick={handleRegister}>Register</button>
        </div>
    )
}

export default RegisterPage;