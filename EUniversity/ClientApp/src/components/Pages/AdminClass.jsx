import {React, useEffect, useState} from 'react';
import { useLocation } from 'react-router-dom';
import { useAppSelector } from '../../store/store';
import Button from "../UI/Button";
import AddItemToGroupModal from '../AddItemToGroupModal';
import DeleteModal from '../DeleteModal';
import Loader from "../UI/Loader";

const AdminClass = () => {

    const [isLoading, setIsLoading] = useState(true);
    const [classItem, setClassItem] = useState(null);
    const [targetDate, setTargetDate] = useState(null);
    const [timeString, setTimeString] = useState("");
    const location = useLocation();
    const regex = /\/classes\/(\d+)/;
    const classNumber = location.pathname.match(regex)[1];

    useEffect(() => {
        fetchClass();
    }, []);

    useEffect(() => {
        const interval = setInterval(() => {
          setTimeString(getTimeDifference(targetDate));
        }, 1000);
        return () => clearInterval(interval);
      }, [targetDate]);

    const getTimeDifference = (targetDate) => {

        const currentDate = new Date();
        const newTargetDate = new Date(targetDate);
        const difference = newTargetDate - currentDate;

        const [hours, minutes] = classItem.duration.split(":").map(Number);
        const currentSeconds = Math.floor((difference % (1000 * 60)));

        const daysDif = Math.floor(difference / (1000 * 60 * 60 * 24));
        const hoursDif = Math.floor((difference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        const minutesDif = Math.floor((difference % (1000 * 60 * 60)) / (1000 * 60));
        const secondsDif = Math.floor((difference % (1000 * 60)) / 1000);

        if ((currentSeconds + hours * 3600 + minutes * 60) < 0) {
            return (' class is already ended');
        }
        else if (currentSeconds < (hours * 3600 + minutes * 60)) {
            return (' class is already started');
        } 
        else {
            return (difference !== null ? 
                ` ${(daysDif > 0 ? `${daysDif} days` : "")} 
                ${hoursDif > 0 ? `${hoursDif} hours` : ""}
                ${minutesDif > 0 ? `${minutesDif} minutes` : ""}
                ${secondsDif > 0 ? `${secondsDif} seconds` : ""}
                `  
                : "")
        }
    };

    const fetchClass = async(page = 1, pageSize = 10) => {
        try {
            const response = await fetch(`/api/classes/${classNumber}`);
            if (response.ok) {
                const data = await response.json();
                setClassItem(data);
                setTargetDate(data.startDate);
                setIsLoading(false);

            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }
    };
    
    const convertTimeFormat = (inputTime) => {
        const date = new Date(inputTime);
        const day = date.getDate().toString().padStart(2, '0');
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const year = date.getFullYear();

        return `${day}.${month}.${year}`;
    };

    return (
        <>
            <div className="students container max-w-[1100px] pt-10">
                <h1 className="students__title form__title">
                    Class #{classNumber}
                </h1>
                <div className="flex justify-between gap-4">
                    {
                        classItem !== null 
                        ?   <>
                                <div className="flex flex-col gap-3">
                                    <h2 className="text-3xl font-bold">type: 
                                        <span className="font-medium"> {classItem.classType.name}</span>
                                    </h2>
                                    <h2 className="text-3xl font-bold">classroom: 
                                        <span className="font-medium"> {classItem.classroom.name}</span>
                                    </h2>
                                    <h2 className="text-3xl font-bold">group: 
                                        <span className="font-medium"> {classItem.group.name}</span>
                                    </h2>
                                    <h2 className="text-3xl font-bold">teacher: 
                                        <span className="font-medium"> {classItem.group.teacher.firstName} {classItem.group.teacher.lastName}</span>
                                    </h2>
                                </div>
                                <div className="flex flex-col gap-3">
                                    <h2 className="text-3xl font-bold">duration: 
                                        <span className="font-medium"> {classItem.duration.slice(0, -3)} min</span>
                                    </h2>
                                    <h2 className="text-3xl font-bold">start at: 
                                        <span className="font-medium"> {convertTimeFormat(classItem.startDate)}</span>
                                    </h2>
                                    <h2 className="text-3xl font-bold">before start: 
                                        <span className="font-medium"> 
                                            {timeString}
                                        </span>
                                    </h2>
                                </div>
                            </>
                        : <Loader/>
                    }
                </div>
                
            </div>
        </>
    );
};

export default AdminClass;  