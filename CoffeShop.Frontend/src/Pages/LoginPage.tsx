import "./LoginPage.css";
import logo from '../assets/CoffeShopLogo.png';
import { type SyntheticEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

const LoginPage = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (event: SyntheticEvent<HTMLFormElement>) => {
        event.preventDefault();
        setError(null);

        const ok = await login(username, password);

        if(ok) {
            navigate ("/menu");
        }
        else{
            setError("User or password are not valid")
        }
    };

    const navigate = useNavigate();
    const {login, status} = useAuth();

    

    return (
    <div className="login-page-wrapper">
        <div className="login">
            <div className="log-welcome">
                <h2>The best coffee of Cognizant</h2>
                <img className="login-logo" src={logo} alt="Company Logo"/>
            </div>
            <div className="log-form">
                <h2>Login</h2>
                <form onSubmit={handleSubmit}>
                    <label htmlFor="username">Username</label>
                    <input
                        id="username"
                        name="username"
                        type="text"
                        placeholder="Enter your username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                    <label htmlFor="password">Password</label>
                    <input
                        id="password"
                        name="password"
                        type="password"
                        placeholder="Enter your password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <button type="submit" disabled={status === "authenticating"}>
                        {status === "authenticating" ? "Charging..." : "Submit"}
                    </button>
                </form>
            </div>
        </div>
    </div>
    );
};

export default LoginPage;
