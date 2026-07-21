import "./LoginPage.css";
import logo from '../assets/CoffeShopLogo.png';
import { type SyntheticEvent, useState } from "react";

const LoginPage = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = (event: SyntheticEvent<HTMLFormElement>) => {
    event.preventDefault();
};

    return (
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

                    <button type="submit">Submit</button>
                </form>
            </div>

        </div>
    );
};

export default LoginPage;
