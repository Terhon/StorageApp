import {BrowserRouter as Router, Link, Route, Routes} from "react-router-dom";
import {ItemTypesFeature} from "./features/itemTypes";
import React from "react";

export function AppRouting() {
    return (
        <Router>
            <div className="container">
                <nav>
                    <Link to="/">Home</Link>
                    <Link to="/item-types">Item Types</Link>
                </nav>

                <Routes>
                    <Route path="/" element={<div>Home</div>}/>
                    <Route path="/item-types" element={<ItemTypesFeature/>}/>
                </Routes>
            </div>

        </Router>
    );
}