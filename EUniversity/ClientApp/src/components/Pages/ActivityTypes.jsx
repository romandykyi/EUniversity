import React from 'react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../UI/Button';
import { useAppSelector } from '../../store/store';
import DeleteModal from '../DeleteModal';
import PageForm from '../PageForm';
import EditFormModal from '../EditFormModal';

const ActivityTypes = () => {

    const [activityTypes, setActivityTypes] = useState([]);
    const [pageSize, setPageSize] = useState(10);
    const [page, setPage] = useState(1);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [inputValue, setInputValue] = useState("");
    const [sortingMethod, setSortingMethod] = useState(0);
    const [isEditable, setIsEditable] = useState(false);
    const [editedItem, setEditedItem] = useState(null);
    const [deletedActivityType, setDeletedActivityType] = useState({
        id: '',
        name: ''
    });
    const navigate = useNavigate();
    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

    const deleteActivityType = async(activityTypeId) => {
        try {
            const response = await fetch(`/api/activityTypes/${activityTypeId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                }
            });
            if (response.ok) {
                setIsDeleteVisible(false);
            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }
    };

    return (
        <PageForm
                setItems={setActivityTypes}
                additionalComponents={
                    <>
                        <DeleteModal
                            isVisible={isDeleteVisible}
                            setIsVisible={setIsDeleteVisible}
                            itemType = "group"
                            deleteFunction = {deleteActivityType}
                            deletedItem = {deletedActivityType}    
                        />
                        {
                            editedItem
                            ?    <EditFormModal
                                    item={editedItem}
                                    isEditable={isEditable}
                                    setIsEditable={setIsEditable}
                                    responseTitle="activityTypes"
                                />
                            : ""
                        }
                    </>
                }
                registerTitle="activityTypes"
                tableBody={(
                    activityTypes.map((item) => (
                        <tr 
                            key={item.id} className="cursor-pointer"
                        >
                            <td>{item.name}</td>
                            {
                            isAdmin
                                ?   <>
                                        <td><Button onClick = {e => 
                                            {
                                                e.stopPropagation();
                                                setIsDeleteVisible(true);
                                                setDeletedActivityType({id: item.id, name: item.name});
                                            }}
                                            >Delete ActivityType</Button></td>
                                        <td>
                                            <Button onClick = {e => 
                                                {
                                                    e.stopPropagation();
                                                    setIsEditable(true);
                                                    setEditedItem(item);
                                                }}
                                            >Edit ActivityType</Button>
                                        </td>
                                    </>
                                :   ""
                        }
                        </tr>
                    ))
                )}
                tableHead={(
                    <tr>
                        <th>Name</th>
                        {
                            isAdmin
                                ?   <>
                                        <th>Delete</th>
                                        <th>Edit</th>
                                    </>
                                : ""
                        }
                    </tr>
                )}
                searchLink={`/api/activityTypes?Page=${page}&PageSize=${pageSize}&name=${inputValue}&sortingMode=${sortingMethod}`}
                fetchLink={`/api/activityTypes?Page=${page}&PageSize=${pageSize}&sortingMode=${sortingMethod}`}
                currentPage={page}
                setCurrentPage={setPage}
                itemsPerPage={pageSize}
                setItemsPerPage={setPageSize}
                inputValue={inputValue}
                setInputValue={setInputValue}
                isDeleteVisible={isDeleteVisible}
                setSortingMethod={setSortingMethod}
                sortingMethod={sortingMethod}
                isEditVisible={isEditable}
        />      
    );
};

export default ActivityTypes;