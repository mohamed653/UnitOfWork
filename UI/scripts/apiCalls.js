$(document).ready(function () {
  // **************Fetching API Calls**************
  LoadAllStudents();
  $.ajax({
    url: "https://10.175.240.71/api/departments",
    type: "GET",
    contentType: "application/json",
    success: function (data) {
      console.log(data);
      loadDepartmentList(data);
    },
    error: function (error) {
      alert("Error");
    },
  });
  $.ajax({
    url: "https://10.175.240.71/api/Students/GetAllSupervisors",
    type: "GET",
    contentType: "application/json",
    success: function (data) {
      console.log(data);
      loadSupervisorList(data);
    },
    error: function (error) {
      alert("Error");
    },
  });
  // **************Posting API Calls**************
  $("#addStudentForm").click(function (event) {
    debugger;
    let studentDto = {
      stName: $("#name").val(),
      stAge: parseInt($("#age").val()),
      stAddress: $("#address").val(),
      deptId: $("#departmentSelect").val(),
      supervisorId: $("#supervisorSelect").val(),
    };
    $.ajax({
      url: "https://10.175.240.71/api/Students/addStudentDto",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(studentDto),
      success: function (data) {
        alert("Student Added Successfully");
        LoadAllStudents();
      },
      error: function (error) {
        console.log(error);
        alert("Error");
      },
    });
  });
});
// **************Functions**************
function LoadAllStudents() {
  $.ajax({
    url: "https://10.175.240.71/api/students", //https://10.175.240.71/api/students
    type: "GET",
    contentType: "application/json",
    success: function (data) {
      console.log(data);
      loadStudentsToTable(data);
    },
    error: function (error) {
      alert("Error");
    },
  });
}

function loadStudentsToTable(students) {
  if (students.length > 0) {
    let tableContainer = document.getElementById("tableContainer");
    tableContainer.innerHTML = "";
    let studentsTable = document.createElement("table");
    studentsTable.setAttribute(
      "class",
      "table table-striped table-bordered table-hover"
    );
    let tableHeader = document.createElement("thead");
    // create it dynamically using object keys
    let headerRow = document.createElement("tr");
    for (let key in students[0]) {
      let header = document.createElement("th");
      header.innerHTML = key;
      headerRow.appendChild(header);
    }
    tableHeader.appendChild(headerRow);
    studentsTable.appendChild(tableHeader);
    let tableBody = document.createElement("tbody");
    students.forEach((student) => {
      let row = document.createElement("tr");
      for (let key in student) {
        let cell = document.createElement("td");
        cell.innerHTML = student[key];
        row.appendChild(cell);
      }
      tableBody.appendChild(row);
    });
    studentsTable.appendChild(tableBody);
    tableContainer.appendChild(studentsTable);
  }
}
function loadDepartmentList(departments) {
  if (departments.length > 0) {
    let departmentSelect = document.getElementById("departmentSelect");
    departments.forEach((department) => {
      let option = document.createElement("option");
      option.value = department.deptId;
      option.text = department.deptName;
      departmentSelect.appendChild(option);
    });
  }
}
function loadSupervisorList(supervisors) {
  if (supervisors.length > 0) {
    let supervisorSelect = document.getElementById("supervisorSelect");
    supervisors.forEach((supervisor) => {
      let option = document.createElement("option");
      option.value = supervisor.stId;
      option.text = supervisor.stName;
      supervisorSelect.appendChild(option);
    });
  }
}
