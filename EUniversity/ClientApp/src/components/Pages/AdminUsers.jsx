import React, { useState } from 'react';
import PageForm from '../PageForm';
import Button from '../UI/Button';
import DeleteModal from '../DeleteModal';

const AdminUsers = () => {
  const [users, setUsers] = useState([]);
  const [usersType, setUsersType] = useState('students');
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [inputValue, setInputValue] = useState("");
  const [isDeleteVisible, setIsDeleteVisible] = useState(false);
  const [sortingMethod, setSortingMethod] = useState(0);
  const [deletedUser, setDeletedUser] = useState({
    id: '',
    name: ''
});

  const deleteUser = async (userId) => {
    try {        
      const response = await fetch(`/api/users/${userId}`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
        }
    });

      if (response.ok) {
        console.log('ok')
      } else {
        console.log('error');
      }
    } catch (error) {
      console.log(error);
    }
  };

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
      searchLink={`/api/users?Page=${page}&PageSize=${pageSize}&FullName=${inputValue}&sortingMode=${sortingMethod}`}
      fetchLink={`/api/users/${usersType}?Page=${page}&PageSize=${pageSize}&sortingMode=${sortingMethod}`}
      tableHead={
        <tr>
              <th>Email</th>
              <th>First name</th>
              <th>Last name</th>
              <th>Middle Name</th>
              <th>Delete user</th> 
        </tr>
      }
      tableBody={users.map((item) => (
        <tr key={item.id}>
          <td>{item.email}</td>
          <td>{item.firstName}</td>
          <td>{item.lastName}</td>
          <td>{item.middleName}</td>
          <td>
            <Button onClick={() => setIsDeleteVisible}>Delete</Button>
          </td>
        </tr>
      ))}
      additionalComponents={
        <DeleteModal
            isVisible={isDeleteVisible}
            setIsVisible={setIsDeleteVisible}
            itemType = "user"
            deleteFunction = {deleteUser}
            deletedItem = {deletedUser}    
        />
      }
      additionalItems = {
        <>
          <select className="form-select students__select mb-0 text-text bg-background" onChange={changeUsersType}>
                <option defaultValue disabled>Users type:</option>
                <option value="students">Students</option>
                <option value="teachers">Teachers</option>
            </select>
        </>
      }
      setSortingMethod={setSortingMethod}
      sortingMethod={sortingMethod}
    />
  );
};

export default AdminUsers;
