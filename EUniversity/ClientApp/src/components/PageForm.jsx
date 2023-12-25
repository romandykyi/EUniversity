import React, { useEffect, useState } from 'react';
import { useAppSelector } from '../store/store';
import AddItemModal from './AddItemModal';
import PageOfItems from './PageOfItems';
import Button from './UI/Button';
import Search from './Search';
import SortSelect from './UI/SortSelect';

const PageForm = ({
    usersType,
    setItems,
    additionalItems,
    additionalComponents,
    registerTitle,
    tableBody,
    tableHead,
    searchLink,
    fetchLink,
    currentPage,
    setCurrentPage,
    itemsPerPage,
    setItemsPerPage,
    inputValue,
    setInputValue,
    isDeleteVisible,
    sortingMethod,
    setSortingMethod,
    isEditVisible,
}) => {
    const [timeoutId, setTimeoutId] = useState(null);
    const [isResponsePossible, setIsResponsePossible] = useState(true);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [isLoading, setIsLoading] = useState(true);
    const [totalItems, setTotalItems] = useState(0);
    const isAdmin = useAppSelector((state) => state.isAdmin.isAdmin);

    useEffect(() => {
        fetchItems(1,10);
    }, []);

    useEffect(() => {
      if (!isDeleteVisible || !isEditVisible) {
        fetchItems(currentPage, itemsPerPage);
      };
    }, [isDeleteVisible, isEditVisible])
    
      useEffect(() => {
        fetchItems(currentPage, itemsPerPage);
      }, [currentPage, usersType, inputValue, totalItems, sortingMethod, fetchLink]);
    
      const fetchItems = async (page = 1, pageSize = 10) => {
        if (inputValue && isResponsePossible) {
          setIsLoading(true);
          if (timeoutId) {
            clearTimeout(timeoutId);
          }
          
          const newTimeoutId = setTimeout(async () => {
              setIsResponsePossible(false);
              try {
                  const response = await fetch(searchLink);
                  
                  if (response.ok) {
                      const data = await response.json();
                      setItems(data.items);
                      setItemsPerPage(data.pageSize);
                      setTotalItems(data.totalItemsCount);
                      setIsLoading(false);
                  } else {
                      console.log('error');
                  }
              } catch (error) {
                  console.log(error);
              } finally {
                  setIsResponsePossible(true);
              }
          }, 500);
    
          setTimeoutId(newTimeoutId);
        }
        else {
          try {
            const response = await fetch(fetchLink);
            if (response.ok) {
              const data = await response.json();
              setItems(data.items);
              setItemsPerPage(data.pageSize);
              setTotalItems(data.totalItemsCount);
              setIsLoading(false);
            } else {
              console.log('error');
            }
          } catch (error) {
            console.log(error);
          }
        }
      };

    return (
        <>
            <AddItemModal
              isVisible={isModalVisible}
              setIsVisible={setIsModalVisible}
              title={registerTitle === "users" ? registerTitle : registerTitle === "classes" ? registerTitle.slice(0, -2) : registerTitle.slice(0, -1)}
              responseTitle={registerTitle}
              fetchItems={fetchItems}
            />
            
            {additionalComponents}
            <PageOfItems
                title={`${inputValue ? "Found" : "All"} ${usersType ? usersType : registerTitle} (${totalItems})`}
                fetchFunction={fetchItems}
                isLoading={isLoading}
                itemsPerPage={itemsPerPage}
                setItemsPerPage={setItemsPerPage}
                totalItems={totalItems}
                itemsType={usersType}
                currentPage={currentPage}
                setCurrentPage={setCurrentPage}
                tableHead={tableHead}
                tableBody={tableBody}
                additionalItems={
                <>
                    {additionalItems}
                    <SortSelect
                      setSortingMethod={setSortingMethod}
                    />
                    <Search
                      setInputValue={setInputValue}
                      search={fetchItems}
                    />
                    {isAdmin && (
                      <Button onClick={() => {
                        setIsModalVisible(true);
                        document.body.style.overflow = 'hidden';
                      }}>Register new {registerTitle === "users" ? registerTitle : registerTitle === "classes" ? registerTitle.slice(0, -2) : registerTitle.slice(0, -1)}</Button>
                    )}
                </>
                }
            />
        </>
    );
};

export default PageForm;