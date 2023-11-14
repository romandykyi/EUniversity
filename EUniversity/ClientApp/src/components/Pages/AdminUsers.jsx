import React, { useState, useEffect } from 'react';
import PageOfItems from '../PageOfItems';
import { useAppSelector } from '../../store/store';
import Button from '../UI/Button/Button';
import AddItemModal from '../UI/AddItemModal/AddItemModal';

const AdminUsers = () => {
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [itemsPerPage, setItemsPerPage] = useState(10);
  const [totalItems, setTotalItems] = useState(0);
  const [usersType, setUsersType] = useState('students');
  const [isModalVisible, setIsModalVisible] = useState(false);
  const isAdmin = useAppSelector((state) => state.isAdmin.isAdmin);

  useEffect(() => {
    fetchUsers();
  }, [usersType]);

  const fetchUsers = async (page = 1, pageSize = 10) => {
    try {
      const response = await fetch(`/api/users/${usersType}?Page=${page}&PageSize=${pageSize}`);
      if (response.ok) {
        const data = await response.json();
        setUsers(data.items);
        setItemsPerPage(data.pageSize);
        setTotalItems(data.totalItemsCount);
        setIsLoading(false);
      } else {
        console.log('error');
      }
    } catch (error) {
      console.log(error);
    }
  };

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
    <>
      <AddItemModal isVisible={isModalVisible} setIsVisible={setIsModalVisible} />
      <PageOfItems
        title={`All ${usersType} (${totalItems})`}
        fetchFunction={fetchUsers}
        isLoading={isLoading}
        itemsPerPage={itemsPerPage}
        setItemsPerPage={setItemsPerPage}
        totalItems={totalItems}
        usersType={usersType}
        tableHead={
          <tr>
            <th>Email</th>
            <th>First name</th>
            <th>Last name</th>
            <th>Middle Name</th>
            {/* <th>Delete user</th> add when DELETE method will be ready */}
          </tr>
        }
        tableBody={
          users.map((item) => (
            <tr key={item.id}>
              <td>{item.email}</td>
              <td>{item.firstName}</td>
              <td>{item.lastName}</td>
              <td>{item.middleName}</td>
              {/* <td>
                <Button onClick={() => deleteUser(item.id)}>Delete</Button> //add when DELETE method will be ready
              </td> */}
            </tr>
          ))
        }
        additionalItems={
          <>
            <select className="form-select students__select mb-0" onChange={changeUsersType}>
                <option value="students">Students</option>
                <option value="teachers">Teachers</option>
            </select>
            {isAdmin && (
              <div>
                <Button onClick={() => setIsModalVisible(true)}>Register new user</Button>
              </div>
            )}
          </>
        }
      />
    </>
  );
};

export default AdminUsers;
