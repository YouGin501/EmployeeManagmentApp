import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

const EmployeeList = () => {
  const [employees, setEmployees] = useState([]);
  const [filteredEmployees, setFilteredEmployees] = useState([]);
  const [departmentFilter, setDepartmentFilter] = useState("all");
  const [positionFilter, setPositionFilter] = useState("all");
  const [nameFilter, setNameFilter] = useState("");
  const [departments, setDepartments] = useState([]);
  const [positions, setPositions] = useState([]);

  useEffect(() => {
    fetch("https://localhost:7233/api/Employees")
      .then((res) => res.json())
      .then((data) => {
        setEmployees(data);
        setFilteredEmployees(data);

        const uniqueDepartments = [
          ...new Set(
            data.map((employee) => employee.department.departmentName)
          ),
        ];
        setDepartments(uniqueDepartments);

        const uniquePositions = [
          ...new Set(data.map((employee) => employee.position.positionName)),
        ];
        setPositions(uniquePositions);
      })
      .catch((error) => console.error("Ошибка получения данных:", error));
  }, []);

  useEffect(() => {
    // Функция для фильтрации
    const filterEmployees = () => {
      let filteredData = employees;

      // Фильтрация по департаменту
      if (departmentFilter !== "all") {
        filteredData = filteredData.filter(
          (employee) => employee.department.departmentName === departmentFilter
        );
      }

      // Фильтрация по позиции
      if (positionFilter !== "all") {
        filteredData = filteredData.filter(
          (employee) => employee.position.positionName === positionFilter
        );
      }

      // Фильтрация по ФИО
      if (nameFilter.trim() !== "") {
        const searchTerm = nameFilter.toLowerCase();
        filteredData = filteredData.filter((employee) =>
          `${employee.firstName.toLowerCase()} ${employee.lastName.toLowerCase()}`.includes(
            searchTerm
          )
        );
      }

      setFilteredEmployees(filteredData);
    };

    filterEmployees();
  }, [departmentFilter, positionFilter, nameFilter, employees]);

  return (
    <div>
      <h1 style={{ textAlign: "center", marginBottom: "20px" }}>
        Employee List
      </h1>
      <div style={{ marginBottom: "20px" }}>
        <b>Filters:&nbsp;&nbsp;&nbsp;&nbsp;</b>
        <label>
          Department:
          <select
            value={departmentFilter}
            onChange={(e) => setDepartmentFilter(e.target.value)}
          >
            <option value="all">All</option>
            {departments.map((department, index) => (
              <option key={index} value={department}>
                {department}
              </option>
            ))}
          </select>
        </label>
        <label style={{ marginLeft: "10px" }}>
          Position:
          <select
            value={positionFilter}
            onChange={(e) => setPositionFilter(e.target.value)}
          >
            <option value="all">All</option>
            {positions.map((position, index) => (
              <option key={index} value={position}>
                {position}
              </option>
            ))}
          </select>
        </label>
        <label style={{ marginLeft: "10px" }}>
          Name:
          <input
            type="text"
            value={nameFilter}
            onChange={(e) => setNameFilter(e.target.value)}
          />
        </label>
      </div>
      <div
        style={{
          display: "flex",
          flexWrap: "wrap",
          justifyContent: "space-around",
        }}
      >
        {filteredEmployees.map((employee) => (
          <Link
            key={employee.id}
            to={`/employees/${employee.id}`}
            style={{ textDecoration: "none", color: "inherit" }}
          >
            <div style={cardStyle}>
              <h2
                style={{ fontSize: "1.5rem", marginBottom: "10px" }}
              >{`${employee.firstName} ${employee.middleName} ${employee.lastName}`}</h2>
              <p>
                <strong>Department:</strong>{" "}
                {employee.department.departmentName}
              </p>
              <p>
                <strong>Position:</strong> {employee.position.positionName}
              </p>
              <p>
                <strong>Salary:</strong> ${employee.salary}
              </p>
              <p>
                <strong>Address:</strong> {employee.address}
              </p>
              <p>
                <strong>Phone Number:</strong> {employee.phoneNumber}
              </p>
              <p>
                <strong>Birth Date:</strong>{" "}
                {employee.birthDate.split(/[- T]/).slice(0, 3).join("/")}
              </p>
              <p>
                <strong>Hire Date:</strong>{" "}
                {employee.hireDate.split(/[- T]/).slice(0, 3).join("/")}
              </p>
              <p>
                <strong>Company Info:</strong> {employee.companyInfo}
              </p>
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
};

const cardStyle = {
  width: "300px",
  borderRadius: "8px",
  boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
  padding: "15px",
  margin: "10px",
  backgroundColor: "#fff",
  transition: "transform 0.3s ease-in-out",
  cursor: "pointer",
  overflow: "hidden",
};

export default EmployeeList;
