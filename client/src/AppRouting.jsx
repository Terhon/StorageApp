import {BrowserRouter as Router, Link, Route, Routes} from "react-router-dom";
import {ItemTypesFeature} from "./features/itemTypes";
import React from "react";
import {StorageItemsFeature} from "./features/storageItems/index.jsx";
import {ChatFeature} from "./features/chat/ChatFeature.jsx";
import Login from "./Login.jsx";
import {ProtectedRoute} from "./auth.jsx";

export function AppRouting() {
    return (
        <Router>
            <div className="container mx-auto p-6">
                <nav className="navbar bg-base-200 rounded-xl shadow mb-6">
                    <div className="flex-1">
                        <Link to="/" className="btn btn-ghost normal-case text-xl">
                            StorageApp
                        </Link>
                    </div>
                    <div className="flex-none">
                        <ul className="menu menu-horizontal px-1">
                            <li>
                                <Link to="/" className="btn btn-ghost normal-case text-xl">Home</Link>
                            </li>
                            <li>
                                <Link to="/item-types" className="btn btn-ghost normal-case text-xl">Item Types</Link>
                            </li>
                            <li>
                                <Link to="/storage-items" className="btn btn-ghost normal-case text-xl">Storage Items</Link>
                            </li>
                            <li>
                                <Link to="/login" className="btn btn-ghost normal-case text-sm">Login</Link>
                            </li>
                        </ul>
                    </div>
                </nav>

                <Routes>
                    <Route path="/" element={<ChatFeature/>}/>
                    <Route path="/item-types" element={<ProtectedRoute><ItemTypesFeature/></ProtectedRoute>}/>
                    <Route path="/storage-items" element={<ProtectedRoute><StorageItemsFeature/></ProtectedRoute>}/>
                    <Route path="/login" element={<Login/>}/>
                </Routes>
            </div>

        </Router>
    );
}