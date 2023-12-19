import { useEffect, useState } from "react";
import jsPDF from "jspdf";
import "jspdf-autotable";
import "./index.css";

const Report = () => {
  const [employees, setEmployees] = useState([]);
  const [filteredEmployees, setFilteredEmployees] = useState([]);
  const [departmentFilter, setDepartmentFilter] = useState("all");
  const [departmentIdFilter, setDepartmentIdFilter] = useState(0);
  const [positionFilter, setPositionFilter] = useState("all");
  const [positionIdFilter, setPositionIdFilter] = useState(0);
  const [startDateFilter, setStartDateFilter] = useState("2023-11-12");
  const [endDateFilter, setEndDateFilter] = useState("2023-12-12");
  const [departments, setDepartments] = useState([]);
  const [positions, setPositions] = useState([]);
  const [filterButtonClicked, setFilterButtonClicked] = useState(false);

  const apiUrl = "https://localhost:7233/api/Employees/GeneralSalaryInfo";

  const calculateDaysDifference = (startDate, endDate, hireDate) => {
    const startDateFilter = new Date(startDate);
    const endDateFilter = new Date(endDate);
    const hireDateObj = new Date(hireDate);

    const timeDifference = hireDateObj - startDateFilter;
    const millisecondsPerDay = 24 * 60 * 60 * 1000;

    const daysDifference = Math.floor(timeDifference / millisecondsPerDay);

    const days =
      daysDifference > 0
        ? Math.floor((endDateFilter - hireDateObj) / millisecondsPerDay)
        : Math.floor((endDateFilter - startDateFilter) / millisecondsPerDay);

    return days;
  };

  function callApi() {
    const urlWithParams = `${apiUrl}?startPeriod=${encodeURIComponent(
      startDateFilter
    )}&endPeriod=${encodeURIComponent(
      endDateFilter
    )}&departmentId=${encodeURIComponent(
      departmentIdFilter
    )}&positionId=${encodeURIComponent(positionIdFilter)}`;

    fetch(urlWithParams)
      .then((res) => res.json())
      .then((data) => {
        setEmployees(data);
        setFilteredEmployees(data);
      });
  }

  useEffect(() => {
    const urlWithParams = `${apiUrl}?startPeriod=${encodeURIComponent(
      startDateFilter
    )}&endPeriod=${encodeURIComponent(
      endDateFilter
    )}&departmentId=${encodeURIComponent(
      departmentIdFilter
    )}&positionId=${encodeURIComponent(positionIdFilter)}`;

    fetch(urlWithParams)
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
    callApi();
    setFilterButtonClicked(false);
  }, [filterButtonClicked]);

  const handleFilterButtonClick = () => {
    setFilterButtonClicked(true);
  };

  function findDepartmentIdByName(departmentName) {
    if (departmentName === "all") {
      return 0;
    }
    const employee = employees.find(
      (emp) => emp.department.departmentName === departmentName
    );
    return employee ? employee.department.id : null;
  }

  function findPositionIdByName(positionName) {
    if (positionName === "all") {
      return 0;
    }
    const employee = employees.find(
      (emp) => emp.position.positionName === positionName
    );
    return employee ? employee.positionId : null;
  }

  const handleDepFilter = (e) => {
    setDepartmentFilter(e.target.value);
    const res = findDepartmentIdByName(e.target.value);
    setDepartmentIdFilter(res);
  };

  const handlePosFilter = (e) => {
    setPositionFilter(e.target.value);
    const res = findPositionIdByName(e.target.value);
    setPositionIdFilter(res);
  };

  const handleDownloadPdf = () => {
    const pdf = new jsPDF();
    pdf.text("Report Page", 105, 10, null, null, "center");

    const headers = [
      "Full Name",
      "Department",
      "Position",
      "Period",
      "Total Days",
      "Total Salary",
    ];
    const data = filteredEmployees.map((employee) => [
      `${employee.firstName} ${employee.middleName} ${employee.lastName}`,
      employee.department.departmentName,
      employee.position.positionName,
      `${
        employee.hireDate > startDateFilter
          ? employee.hireDate.split(/[- T]/).slice(0, 3).join("-")
          : startDateFilter
      } => ${endDateFilter}`,
      calculateDaysDifference(
        startDateFilter,
        endDateFilter,
        employee.hireDate
      ),
      `$${employee.salary}`,
    ]);

    pdf.autoTable({
      head: [headers],
      body: data,
      startY: 20,
    });

    pdf.save("report.pdf");
  };

  const handleDownloadTxt = () => {
    const data = [];
    data.push(
      "Full Name\tDepartment\tPosition\tStart Period(Hire date) => End Period\tTotal Days\tTotal Salary"
    );

    filteredEmployees.forEach((employee) => {
      const row = [
        `${employee.firstName} ${employee.middleName} ${employee.lastName}`,
        employee.department.departmentName,
        employee.position.positionName,
        `${
          employee.hireDate > startDateFilter
            ? employee.hireDate.split(/[- T]/).slice(0, 3).join("-")
            : startDateFilter
        } => ${endDateFilter}`,
        calculateDaysDifference(
          startDateFilter,
          endDateFilter,
          employee.hireDate
        ),
        `$${employee.salary}`,
      ];
      data.push(row.join("\t"));
    });

    const txtContent = data.join("\n");
    const blob = new Blob([txtContent], { type: "text/plain" });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = "report.txt";
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.URL.revokeObjectURL(url);
  };

  return (
    <div>
      <h2 style={{ textAlign: "center" }}>Report Page</h2>
      <div style={{ marginBottom: "20px" }}>
        <label>
          Department:
          <select value={departmentFilter} onChange={(e) => handleDepFilter(e)}>
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
          <select value={positionFilter} onChange={(e) => handlePosFilter(e)}>
            <option value="all">All</option>
            {positions.map((position, index) => (
              <option key={index} value={position}>
                {position}
              </option>
            ))}
          </select>
        </label>
        <label style={{ marginLeft: "10px" }}>
          Start Date:
          <input
            type="date"
            name="startDate"
            value={startDateFilter}
            onChange={(e) => setStartDateFilter(e.target.value)}
          />
        </label>
        <label style={{ marginLeft: "10px" }}>
          End Date:
          <input
            type="date"
            name="endDate"
            value={endDateFilter}
            onChange={(e) => setEndDateFilter(e.target.value)}
          />
        </label>
        <button
          style={{
            marginLeft: "10px",
            color: "white",
            backgroundColor: "black",
            fontWeight: "bold",
          }}
          onClick={handleFilterButtonClick}
        >
          Filter
        </button>
      </div>
      <table className="excel-table">
        <thead>
          <tr>
            <th>Full Name</th>
            <th>Department</th>
            <th>Position</th>
            <th>Start Period(Hire date) =&gt; End Period</th>
            <th>Total Days</th>
            <th>Total Salary</th>
          </tr>
        </thead>
        <tbody>
          {filteredEmployees.map((employee) => (
            <tr key={employee.id}>
              <td>{`${employee.firstName} ${employee.middleName} ${employee.lastName}`}</td>
              <td>{employee.department.departmentName}</td>
              <td>{employee.position.positionName}</td>
              <td>
                {employee.hireDate > startDateFilter
                  ? employee.hireDate.split(/[- T]/).slice(0, 3).join("-")
                  : startDateFilter}{" "}
                =&gt; {endDateFilter}
              </td>
              <td>
                {calculateDaysDifference(
                  startDateFilter,
                  endDateFilter,
                  employee.hireDate
                )}
              </td>
              <td>${employee.salary}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <div style={{ display: "flex" }}>
        <button onClick={handleDownloadPdf} className="download-button">
          Download as PDF
        </button>
        <button onClick={handleDownloadTxt} className="download-button">
          Download as Txt
        </button>
      </div>
    </div>
  );
};

export default Report;
