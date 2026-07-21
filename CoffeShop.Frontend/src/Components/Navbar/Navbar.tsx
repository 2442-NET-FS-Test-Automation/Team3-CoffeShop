import './Navbar.css';
import logo from '../../assets/CoffeShopLogo.png';

const Navbar = () => {
    return (
        <nav className="navbar">
            <div className="nav-brand">
                <img className="nav-logo" src={logo} alt="Company Logo" />
                <span className="nav-title">Coffe Shop Revanture</span>
            </div>

            <div className="nav-menu">
                <button className="nav-link">Menu</button>
                <button className="nav-link">Orders</button>
                <button className="nav-link">Inventory</button>
            </div>

            <div className="nav-actions">
                <button className="login button">Login</button>
                <button className="register button">Register</button>
            </div>
        </nav>
    );
};

export default Navbar;
