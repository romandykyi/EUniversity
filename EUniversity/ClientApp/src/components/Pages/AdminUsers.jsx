import React, {  useState} from 'react';
import PageOfItems from '../PageOfItems';

const AdminUsers = () => {

    const [students, setStudents] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const [usersType, setUsersType] = useState('students');

    const fetchUsers = async(page = 1, pageSize = 10) => {
        try {
            const response = await fetch(`/api/users/${usersType}?Page=${page}&PageSize=${pageSize}`);
            if (response.ok) {
                const data = await response.json();
                setStudents(data.items);
                setItemsPerPage(data.pageSize);
                setTotalItems(data.totalItemsCount);
                setIsLoading(false);

            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }
    };
    
    const changeUsersType = async (e) => {
        const newUserType = e.target.value;
        setUsersType(newUserType);
        //await fetchUsers();
    };
    

    return (
        <>
            <PageOfItems
                 title = {`All ${usersType} (${totalItems})`}
                 fetchFunction = {fetchUsers}
                 isLoading = {isLoading}
                 itemsPerPage = {itemsPerPage}
                 setItemsPerPage = {setItemsPerPage}
                 totalItems = {totalItems}
                 usersType={usersType}
                 tableHead = {(
                    <tr>
                        <th>Email</th>
                        <th>First name</th>
                        <th>Last name</th>
                        <th>Middle Name</th>
                    </tr>
                 )}
                 tableBody = {(
                    students.map((item) => (
                        <tr key={item.id}>
                            <td>{item.email}</td>
                            <td>{item.firstName}</td>
                            <td>{item.lastName}</td>
                            <td>{item.middleName}</td>
                        </tr>
                    ))
                 )}
                 additionalItems = {(
                    <div className="students__select_wrapper">
                        <select className="form-select students__select" onChange={changeUsersType}>
                            <option value="students">Students</option>
                            <option value="teachers">Teachers</option>
                        </select>
                    </div>
                 )}
            />
        </>
    );
};

export default AdminUsers;