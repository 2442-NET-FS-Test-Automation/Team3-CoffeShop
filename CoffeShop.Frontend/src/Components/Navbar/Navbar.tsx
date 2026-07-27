import './Navbar.css';
import logo from '../../assets/CoffeShopLogo.png';
import { useNavigate, Outlet } from "react-router-dom";
import { useAuth } from '../../auth/useAuth';

const Navbar = () => {
    
    const navigate = useNavigate();
    const {user, logout} = useAuth();

    return (
        <>
        <nav className="navbar">
            <div className="nav-brand">
                <img className="nav-logo" src={logo} alt="Company Logo" />
                <span className="nav-title">Coffe Shop Revanture</span>
            </div>

            <div className="nav-menu">
                <button className="nav-link" onClick={() => navigate ('/menu')}>Menu</button>
                <button className="nav-link" onClick={() => navigate ('/inventory')}>Inventory</button>
                <button className="nav-link" onClick={() => navigate ('/dashboard')}>Dashboard</button>
            </div>

            <div className="nav-actions">
                {!user && (
                    <button className="login button" onClick={() => navigate ('/')}>Login</button>
                )}

                {user?.role === "Manager" && (
                    <button className="register button" onClick={() => navigate ('/register')}>Register</button>
                )}

                {user && (
                    <button className="login button" onClick={() => {
                        logout();
                        navigate("/");
                    }}>
                        Logout
                    </button>
                )}
                
            </div>
        </nav>
        <main>
            <Outlet />
        </main>
        </>
    );
};

export default Navbar;
