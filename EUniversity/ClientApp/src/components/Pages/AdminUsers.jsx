import React, { useState } from 'react';
import PageForm from '../PageForm';

const AdminUsers = () => {
  const [users, setUsers] = useState([]);
  const [usersType, setUsersType] = useState('students');
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [inputValue, setInputValue] = useState("");

  // const deleteUser = async (userId) => {
  //   try {
  //     setUsers((prevUsers) => prevUsers.filter((user) => user.id !== userId));           //add when DELETE method will be ready
  //     const response = await fetch(`/api/users/${usersType}`, {
  //       method: "POST",
  //       headers: {
  //           "Content-Type": "application/json",
  //       },
  //       body: `{"users": ${JSON.stringify(users)}}`,
  //   });

  //     if (response.ok) {
  //       console.log('ok')
  //     } else {
  //       console.log('error');
  //     }
  //   } catch (error) {
  //     console.log(error);
  //   }
  // };

  const changeUsersType = (e) => {
    const newUserType = e.target.value;
    setUsersType(newUserType);
  };

  return (
    <PageForm
      currentPage={page}
      itemsPerPage={pageSize}
      inputValue={inputValue}
      setItems={setUsers}
      usersType={usersType}
      setCurrentPage={setPage}
      setInputValue={setInputValue}
      setItemsPerPage={setPageSize}
      registerTitle="users"
      searchLink={`/api/users?Page=${page}&PageSize=${pageSize}&FullName=${inputValue}`}
      fetchLink={`/api/users/${usersType}?Page=${page}&PageSize=${pageSize}`}
      tableHead={
        <tr>
              <th>Email</th>
              <th>First name</th>
              <th>Last name</th>
              <th>Middle Name</th>
              {/* <th>Delete user</th> add when DELETE method will be ready */}
        </tr>
      }
      tableBody={users.map((item) => (
        <tr key={item.id}>
          <td>{item.email}</td>
          <td>{item.firstName}</td>
          <td>{item.lastName}</td>
          <td>{item.middleName}</td>
          {/* <td>
            <Button onClick={() => deleteUser(item.id)}>Delete</Button> //add when DELETE method will be ready
          </td> */}
        </tr>
      ))}
      additionalItems = {
        <>
          <select className="form-select students__select mb-0" onChange={changeUsersType}>
                <option value="students">Students</option>
                <option value="teachers">Teachers</option>
            </select>
        </>
      }
    />
  );
};

export default AdminUsers;
