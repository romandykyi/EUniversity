import React from 'react';
import PageOfItems from '../PageOfItems';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import DeleteModal from '../UI/DeleteModal';
import { useAppSelector } from '../../store/store';
import Button from '../UI/Button';
import AddClassroomModal from '../UI/AddClassroomModal';
import Search from '../Search';

const AdminClassrooms = () => {

    const [classrooms, setClassrooms] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [isAddVisible, setIsAddVisible] = useState(false);
    const [inputValue, setInputValue] = useState("");
    const [foundClassrooms, setFoundClassrooms] = useState([]);
    const [deletedClassroom, setDeletedClassroom] = useState({
        id: '',
        name: ''
    });
    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);
    const navigate = useNavigate();

    const fetchClassrooms = async(page = 1, pageSize = 10) => {

        try {
            const response = await fetch(`/api/classrooms?Page=${page}&PageSize=${pageSize}`);
            if (response.ok) {
                const data = await response.json();
                setClassrooms(data.items);
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

    const deleteClassroom = async(classroomId) => {
        try {
            const response = await fetch(`/api/classrooms/${classroomId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                }
            });
            if (response.ok) {
                console.log(`deleted Classroom: ${deletedClassroom.name}`);
                fetchClassrooms();
                setIsDeleteVisible();
            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }
    };



    return (
        <>
            <DeleteModal
                isVisible={isDeleteVisible}
                setIsVisible={setIsDeleteVisible}
                itemType = "classroom"
                deleteFunction = {deleteClassroom}
                deletedUser = {deletedClassroom}
            />
            <AddClassroomModal
                isVisible={isAddVisible}
                setIsVisible={setIsAddVisible}
                title="classroom"
                responseTitle="classrooms"
                fetchItems={fetchClassrooms}
            />
            <PageOfItems
                title = {`All Classrooms (${totalItems})`}
                fetchFunction = {fetchClassrooms}
                isLoading = {isLoading}
                itemsPerPage = {itemsPerPage}
                setItemsPerPage = {setItemsPerPage}
                totalItems = {totalItems}
                tableHead = {(
                    <tr>
                        <th>Name</th>
                        {
                            isAdmin 
                            ? <th>Delete</th>
                            : ""
                        }
                    </tr>
                )}
                tableBody = {(
                    (inputValue.length ? foundClassrooms : classrooms).map((item) => (
                        <tr 
                            onClick={() => {
                                navigate(`${item.id}`);
                            }} 
                            key={item.id} className="cursor-pointer"
                        >
                            <td>{item.name}</td>
                            {
                                isAdmin 
                                    ? <th><Button onClick = {e => {
                                        e.stopPropagation();
                                        setIsDeleteVisible(true);
                                        setDeletedClassroom({id: item.id, name: item.name});
                                    }}>Delete Classroom</Button></th>
                                    : ""
                            }
                        </tr>
                    ))
                )}
                additionalItems={
                    <>
                        <Search
                            inputValue={inputValue}
                            setInputValue={setInputValue}
                            setFoundItems={setFoundClassrooms}
                            link="/api/classrooms?name="
                        />
                        <Button onClick={() => setIsAddVisible(true)}>Add classrooms</Button>
                    </>
                  }
            />   
        </>
    );
};

export default AdminClassrooms;