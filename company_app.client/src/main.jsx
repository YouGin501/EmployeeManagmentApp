import ReactDOM from "react-dom/client";
import "./index.css";
import Home from "./Home.jsx";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Employees from "./Employees.jsx";
import Report from "./Report.jsx";
import EmployeeDetails from "./EmployeeDetails.jsx";

ReactDOM.createRoot(document.getElementById("root")).render(
  <Router>
    <header>
      <Link className="site-logo" to="/">
        Home
      </Link>
      <nav>
        <Link to="/employees">Employees</Link>
      </nav>
      <nav>
        <Link to="/reports">Reports</Link>
      </nav>
    </header>
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/employees" element={<Employees />} />
      <Route path="/reports" element={<Report />} />
      <Route path="/employees/:id" element={<EmployeeDetails />} />
    </Routes>
  </Router>
);
