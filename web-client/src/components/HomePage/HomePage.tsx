import { useEffect, useState } from "react";
import { fetchHome } from "../../services/api";
import { useNavigate } from "react-router";

function HomePage(){
    const [response, setResponse] = useState("");
    const navigate = useNavigate();
    useEffect(() => {
        fetchHome().then((data: string) => {
            setResponse(data);
        });
    }, []);

    return (
        <div>
            <p>{response}</p>
            <button onClick={() => navigate("/login")}>Login</button>
            <button onClick={() => navigate("/register")}>Register</button>
        </div>
    )
}

export default HomePage;