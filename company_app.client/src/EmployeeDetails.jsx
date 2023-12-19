import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";

const EmployeeDetails = () => {
  const { id } = useParams();
  const [employee, setEmployee] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [editedEmployee, setEditedEmployee] = useState(null);

  const navigate = useNavigate();

  // Gets Data to page
  useEffect(() => {
    fetch(`https://localhost:7233/api/Employees/${id}`)
      .then((res) => res.json())
      .then((data) => setEmployee(data))
      .catch((error) => console.error("Ошибка получения данных:", error));
  }, [id]);

  useEffect(() => {
    if (employee) {
      setEditedEmployee({ ...employee });
    }
  }, [employee]);

  const handleEditClick = () => {
    setIsEditing(true);
  };

  // Sends PUT Request
  const handleSaveClick = () => {
    setIsEditing(false);

    setEmployee({ ...editedEmployee });
    fetch(`https://localhost:7233/api/Employees/${id}`, {
      method: "PUT",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(editedEmployee),
    })
      .then((res) => {
        if (!res.ok) {
          throw new Error(`HTTP error! Status: ${res.status}`);
        }
        return res.json();
      })
      .then((data) => {
        setEmployee(data);
        setIsEditing(false);
      })
      .catch((error) =>
        console.error("Ошибка при сохранении изменений:", error)
      );
  };

  const handleCancelClick = () => {
    setIsEditing(false);
    setEditedEmployee(employee);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setEditedEmployee((prevEmployee) => ({
      ...prevEmployee,
      [name]: value,
    }));
  };

  const handleDeleteClick = () => {
    fetch(`https://localhost:7233/api/Employees/${id}`, {
      method: "DELETE",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({}),
    });
    setIsEditing(false);
    alert("Employee DELETED. Go back to employees page");
    navigate("/employees");
  };

  if (!employee) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>Employee Details</h1>
      {isEditing ? (
        <div style={cardStyle}>
          <label>
            Name:
            <br />
            <input
              type="text"
              name="firstName"
              value={editedEmployee.firstName}
              onChange={handleInputChange}
            />
            <br />
            <input
              type="text"
              name="middleName"
              value={editedEmployee.middleName}
              onChange={handleInputChange}
            />
            <br />
            <input
              type="text"
              name="lastName"
              value={editedEmployee.lastName}
              onChange={handleInputChange}
            />
          </label>
          <br />
          <label>
            Department:
            <br />
            <input
              type="text"
              name="departmentName"
              value={editedEmployee.department.departmentName}
              onChange={handleInputChange}
            />
          </label>
          <br />
          <label>
            Position:
            <br />
            <input
              type="text"
              name="positionName"
              value={editedEmployee.position.positionName}
              onChange={handleInputChange}
            />
          </label>
          <br />
          <label>
            Salary:
            <br />
            <input
              type="number"
              name="salary"
              value={editedEmployee.salary}
              onChange={handleInputChange}
            />
          </label>
          <br />
          <label>
            Address:
            <br />
            <input
              type="text"
              name="address"
              value={editedEmployee.address}
              onChange={handleInputChange}
            />
          </label>
          <br />
          <label>
            Phone number:
            <br />
            <input
              type="text"
              name="phoneNumber"
              value={editedEmployee.phoneNumber}
              onChange={handleInputChange}
            />
          </label>
          <br />
          <label>
            Birth Date:
            <br />
            <input
              type="date"
              name="birthDate"
              value={editedEmployee.birthDate.slice(0, 10)}
              onChange={handleInputChange}
            />
          </label>
          <br />
          <label>
            Hire Date:
            <br />
            <input
              type="date"
              name="hireDate"
              value={editedEmployee.hireDate.slice(0, 10)}
              onChange={handleInputChange}
            />
          </label>
          <br />
          <label>
            Company Info:
            <br />
            <input
              type="text"
              name="companyInfo"
              value={editedEmployee.companyInfo}
              onChange={handleInputChange}
            />
          </label>
          <br />
          <br />
          <button onClick={handleSaveClick}>Save</button>
          <button onClick={handleCancelClick}>Cancel</button>
        </div>
      ) : (
        <div>
          <p>
            <strong>Name:</strong>{" "}
            {`${employee.firstName} ${employee.middleName} ${employee.lastName}`}
          </p>
          <p>
            <strong>Department:</strong> {employee.department.departmentName}
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
          <button onClick={handleEditClick}>Edit</button>
          <button onClick={handleDeleteClick}>Delete</button>
        </div>
      )}
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

  ":hover": {
    transform: "scale(1.05)",
  },
};
export default EmployeeDetails;
