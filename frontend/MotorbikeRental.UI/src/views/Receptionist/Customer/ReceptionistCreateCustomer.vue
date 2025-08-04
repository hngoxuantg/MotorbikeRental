<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import { customerService } from '@/api/services/customerService';
import { useRouter } from 'vue-router'
import CustomerCreate from '../../../components/Receptionist/Customer/CustomerCreate.vue';

const router = useRouter()

async function handleSubmit(params) {
   try {
       await customerService.createCustomer(params)
       alert('Tạo khách hàng thành công!')
       router.push({ name: 'ReceptionistListCustomer' })
   } catch (error) {
       console.error('Failed to create customer:', error)
       alert('Đã xảy ra lỗi khi tạo khách hàng. Vui lòng thử lại sau.')
   }
}
</script>
<template>
    <ReceptionistLayout>
        <CustomerCreate @submit="handleSubmit" />
    </ReceptionistLayout>
</template>