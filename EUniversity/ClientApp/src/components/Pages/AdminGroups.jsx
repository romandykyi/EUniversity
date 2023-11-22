import React from 'react';
import PageOfItems from '../PageOfItems';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../UI/Button';
import { useAppSelector } from '../../store/store';
import DeleteModal from '../UI/DeleteModal';
import AddGroupModal from "../UI/AddGroupModal";
import SearchSelect from '../UI/SearchSelect';
import Search from '../Search';

const AdminGroup = () => {

    const [groups, setGroups] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const [isEditable, setIsEditable] = useState(false);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [isAddVisible, setIsAddVisible] = useState(false);
    const [inputValue, setInputValue] = useState("");
    const [foundGroups, setFoundGroups] = useState([]);
    const [deletedGroup, setDeletedGroup] = useState({
        id: '',
        name: ''
    });
    const navigate = useNavigate();
    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

    

    const fetchGroups = async(page = 1, pageSize = 10) => {

        try {
            const response = await fetch(`/api/groups?Page=${page}&PageSize=${pageSize}`);
            if (response.ok) {
                const data = await response.json();
                setGroups(data.items);
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

    const deleteGroup = async(groupId) => {
        try {
            const response = await fetch(`/api/groups/${groupId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                }
            });
            if (response.ok) {
                console.log(`deleted group: ${deletedGroup.name}`);
                fetchGroups();
                setIsDeleteVisible(false);
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
                itemType = "group"
                deleteFunction = {deleteGroup}
                deletedUser = {deletedGroup}
            />
            <AddGroupModal
                isVisible={isAddVisible}
                setIsVisible={setIsAddVisible}
                title="group"
                responseTitle="groups"
                fetchItems={fetchGroups}
            />
            <PageOfItems
                title = "All Groups"
                fetchFunction = {fetchGroups}
                isLoading = {isLoading}
                itemsPerPage = {itemsPerPage}
                setItemsPerPage = {setItemsPerPage}
                totalItems = {totalItems}
                additionalItems={(
                    <>
                        <Search
                            inputValue={inputValue}
                            setInputValue={setInputValue}
                            setFoundItems={setFoundGroups}
                            link="/api/groups?name="
                        />
                        {
                            isAdmin
                            ? <Button onClick={() => setIsAddVisible(true)}>Add new group</Button>
                            : ""
                        }
                    </>
                )}
                tableHead = {(
                    <tr>
                        <th>Name</th>
                        <th>Course</th>
                        <th>Teacher</th>
                        <th>Teacher username</th>
                        {
                            isAdmin
                                ? <>
                                    {/* <th>Edit</th> */}
                                    <th>Delete</th>
                                  </>
                                : ""
                        }
                    </tr>
                )}
                tableBody = {(
                        (inputValue.length ? foundGroups : groups).map((item) => (
                            <tr 
                                onClick={() => {
                                    navigate(`${item.id}`);
                                }} 
                                key={item.id} className="cursor-pointer"
                            >
                                <td>{item.name}</td>
                                <td>{item.course.name}</td>
                                <td>{item.teacher.firstName} {item.teacher.lastName}</td>
                                <td>{item.teacher.userName}</td>
                                {
                                isAdmin
                                    ? <>
                                        {/* {
                                            isEditable
                                            ? <th className="flex gap-2 items-center">
                                                <Button onClick = {e => {
                                                    e.stopPropagation();
                                                    setIsEditable(false);
                                                }}>Save</Button>
                                                <Button onClick = {e => {
                                                    e.stopPropagation();                //add when it will be search teachers by id method
                                                    setIsEditable(false);
                                                }}>Cancel</Button>
                                              </th>
                                            : <th><Button onClick = {e => {
                                                e.stopPropagation();
                                                setIsEditable(true);
                                            }}>Edit</Button></th>
                                        } */}
                                        <th><Button onClick = {e => {
                                            e.stopPropagation();
                                            setIsDeleteVisible(true);
                                            setDeletedGroup({id: item.id, name: item.name});
                                        }}>Delete Group</Button></th>
                                      </>
                                    : ""
                            }
                            </tr>
                        ))
                    )}
            />   
        </>
    );
};

export default AdminGroup;