import React from 'react';



const Loader = () => {
    return (
    <div className="py-20 flex items-center justify-center">
        <div className='flex w-28 h-28 border-t-theme animate-spin rounded-full border-8'></div>
    </div>

    );
};

export default Loader;