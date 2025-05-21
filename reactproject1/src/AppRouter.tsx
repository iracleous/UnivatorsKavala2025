import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Home from "./pages/Home";
import About from "./pages/About";
import AppWeather from "./pages/AppWeather";


const AppRouter: React.FC = () => {
    return (
        <Router>
            <div style={{ textAlign: "center", margin: "20px" }}>
                <h1>Central Navigation Page</h1>
                <nav>
                    <Link to="/" style={{ margin: "10px" }}>Home</Link>
                    <Link to="/AppWeather" style={{ margin: "10px" }}>AppWeather</Link>
                    <Link to="/about" style={{ margin: "10px" }}>About</Link>
                    
                </nav>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/AppWeather" element={<AppWeather />} />
                    <Route path="/about" element={<About />} />
                </Routes>
            </div>
        </Router>
    );
};

export default AppRouter;
