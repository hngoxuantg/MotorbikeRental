const CUSTOMER_KEY = 'selectedCustomer';
const MOTORBIKE_KEY = 'selectedMotorbike';

export const contractStorage = {
    setCustomer(customerId){
        localStorage.setItem(CUSTOMER_KEY, customerId);
    },
    getCustomer(){
        return localStorage.getItem(CUSTOMER_KEY);
    },
    setMotorbike(motorbikeId){
        localStorage.setItem(MOTORBIKE_KEY, motorbikeId);
    },
    getMotorbike(){
        return localStorage.getItem(MOTORBIKE_KEY);
    },
    clear(){
        localStorage.removeItem(CUSTOMER_KEY);
        localStorage.removeItem(MOTORBIKE_KEY);
    }
}