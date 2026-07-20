import React, { useState } from 'react';
import './Navbar.css';

const Navbar = () => {
    const [isOpen, setIsOpen] = useState(false);
    return(
        <nav className="navbar">
            <div className = "nav-logo">CoffeShopRevanture</div>
            <div className = "nav-buttons">
                <button className="login button">Login</button>
                <bgutton className="register button">Register</button>
            </div>
        </nav>
    )
}

export default Navbar;