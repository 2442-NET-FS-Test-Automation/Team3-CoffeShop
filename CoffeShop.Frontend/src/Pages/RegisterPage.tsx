import "./LoginPage.css";
import logo from '../assets/CoffeShopLogo.png';
import { type SyntheticEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Register } from "../api/auth";

const RegisterPage = () => {

    const navigate = useNavigate();
    const [username, setUsername] = useState("");
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [message, setMessage] = useState<{ text: string, type: 'error' | 'succes'} | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const handleSubmit = async (event: SyntheticEvent<HTMLFormElement>) => {
        event.preventDefault();
        setMessage(null);
        setIsLoading(true);

        if(password !== confirmPassword) {
            setMessage({ text: "The passwords doesn't match", type: "error"});
            setIsLoading(false);
            return;
        }

        try{
            await Register (username, password, email, name);
            setMessage({ text: "The barista was succesfully registered!", type: "succes" });

            setUsername("");
            setPassword(""); 
            setConfirmPassword("");
            setEmail("");
            setName("");
        
        }catch (error) {
            setMessage({ text: "There was an issue registering.", type: "error"});
        }
        finally{
            setIsLoading(false);
        }
    };

    return (
    <div className="login-page-wrapper">
                    <button type="button" className="back-button" onClick={() => navigate('/menu')} >
                        Back to menu
                    </button>
        <div className="login"> 
            
            <div className="log-welcome">
                <h2>Join the best coffee of Cognizant</h2>
                <img className="login-logo" src={logo} alt="Company Logo"/>
            </div>
            
            <div className="log-form">
                <h2>Register</h2>
                <form onSubmit={handleSubmit}>
                    
                    <label htmlFor="username">Username</label>
                    <input
                        id="username"
                        name="username"
                        type="text"
                        placeholder="Choose your username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />

                    <label htmlFor="name">Name</label>
                    <input
                        id="name"
                        name="name"
                        type="text"
                        placeholder="Write your name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        required
                    />

                    <label htmlFor="email">Email</label>
                    <input
                        id="email"
                        name="email"
                        type="email"
                        placeholder="Enter your email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />

                    <label htmlFor="password">Password</label>
                    <input
                        id="password"
                        name="password"
                        type="password"
                        placeholder="Create a password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />

                    <label htmlFor="confirmPassword">Confirm Password</label>
                    <input
                        id="confirmPassword"
                        name="confirmPassword"
                        type="password"
                        placeholder="Confirm your password"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        required
                    />

                    {message && (
                        <p>
                            {message.text}
                        </p>
                    )}

                    <button type="submit" disabled={isLoading}>
                        {isLoading ? "Creating account..." : "Create Account"}
                    </button>
                </form>
            </div>
        </div>
    </div>
    );
};

export default RegisterPage;