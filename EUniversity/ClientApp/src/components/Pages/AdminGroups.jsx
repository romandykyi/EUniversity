import React, {useEffect, useState} from 'react';
import PaginatedList from "../PaginatedList";
import { useNavigate } from 'react-router-dom';
import { useAppDispatch } from '../../store/store';
import { changeCurrentGroup } from '../../store/features/groupSlice';

const AdminGroups = () => {

    const [groups, setGroups] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

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



    return (
        <div className="students container max-w-[1100px] pt-10">
            <h1 className="students__title form__title">
                All Groups
            </h1>
            <PaginatedList
                itemsPerPage={itemsPerPage}
                setItemsPerPage={setItemsPerPage}
                isLoading={isLoading}
                totalItems={totalItems}
                fetchItems={fetchGroups}
                usersType='groups'
                tableHead={(
                    <tr>
                        <th>Name</th>
                        <th>Course</th>
                        <th>Teacher</th>
                        <th>Teacher username</th>
                    </tr>
                )}
                tableBody={(
                    groups.map((item) => (
                        <tr 
                            onClick={() => {
                                navigate(`${item.id}`);
                                dispatch(changeCurrentGroup({id: item.id, name: item.name}));
                            }} 
                            key={item.id} className="cursor-pointer"
                        >
                            <td>{item.name}</td>
                            <td>{item.course.name}</td>
                            <td>{item.teacher.firstName} {item.teacher.lastName}</td>
                            <td>{item.teacher.userName}</td>
                        </tr>
                    ))
                )}
            />
        </div>

    );
};

export default AdminGroups;