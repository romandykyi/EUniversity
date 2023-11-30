import React from 'react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../UI/Button';
import { useAppSelector } from '../../store/store';
import DeleteModal from '../DeleteModal';
import PageForm from '../PageForm';

const AdminSemesters = () => {

    const [semesters, setSemesters] = useState([]);
    const [pageSize, setPageSize] = useState(10);
    const [page, setPage] = useState(1);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [inputValue, setInputValue] = useState("");
    const [sortingMethod, setSortingMethod] = useState(0);
    const [deletedSemester, setDeletedSemester] = useState({
        id: '',
        name: ''
    });
    const navigate = useNavigate();
    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

    


    const deleteSemester = async(semesterId) => {
        try {
            const response = await fetch(`/api/semesters/${semesterId}`, {
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
                setItems={setSemesters}
                additionalComponents={
                    <DeleteModal
                        isVisible={isDeleteVisible}
                        setIsVisible={setIsDeleteVisible}
                        itemType = "semester"
                        deleteFunction = {deleteSemester}
                        deletedItem = {deletedSemester}
                    />
                }
                registerTitle="semesters"
                tableBody={(
                    semesters.map((item) => (
                        <tr 
                            onClick={() => {
                                navigate(`${item.id}`);
                            }} 
                            key={item.id} className="cursor-pointer"
                        >
                            <td>{item.name}</td>
                           <td>{item.dateFrom}</td>
                           <td>{item.dateTo}</td>
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
                                        setDeletedSemester({id: item.id, name: item.name});
                                    }}>Delete Semester</Button></th>
                                  </>
                                : ""
                        }
                        </tr>
                    ))
                )}
                tableHead={(
                    <tr>
                        <th>Name</th>
                        <th>From</th>
                        <th>To</th>
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
                searchLink={`/api/semesters?Page=${page}&PageSize=${pageSize}&name=${inputValue}&sortingMode=${sortingMethod}`}
                fetchLink={`/api/semesters?Page=${page}&PageSize=${pageSize}&sortingMode=${sortingMethod}`}
                currentPage={page}
                setCurrentPage={setPage}
                itemsPerPage={pageSize}
                setItemsPerPage={setPageSize}
                inputValue={inputValue}
                setInputValue={setInputValue}
                isDeleteVisible={isDeleteVisible}
                setSortingMethod={setSortingMethod}
                sortingMethod={sortingMethod}
            />      
    );
};

export default AdminSemesters;
