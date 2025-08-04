import { createRouter, createWebHistory } from 'vue-router'
import { jwtDecode } from 'jwt-decode'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Login',
      component: () => import('@/views/Auth/Login.vue'),
    },
    {
      path: '/admin',
      children: [
        {
          path: 'index',
          name: 'AdminMotorbikeList',
          component: () => import('@/views/Admin/Motorbike/Index.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'motorbike/create',
          name: 'AdminMotorbikeCreate',
          component: () => import('@/views/Admin/Motorbike/CreateMotorbike.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'motorbike/detail/:id',
          name: 'AdminMotorbikeDetail',
          component: () => import('@/views/Admin/Motorbike/DetailMotorbike.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'motorbike/edit/:id',
          name: 'AdminMotorbikeEdit',
          component: () => import('@/views/Admin/Motorbike/EditMotorbike.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'employees',
          name: 'AdminEmployeeList',
          component: () => import('@/views/Admin/Employee/AdminEmployeeList.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'employees/create',
          name: 'AdminEmployeeCreate',
          component: () => import('@/views/Admin/Employee/AdminEmployeeCreate.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'employees/edit/:id',
          name: 'AdminEmployeeEdit',
          component: () => import('@/views/Admin/Employee/AdminEmployeeEdit.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'employees/create-user/:id',
          name: 'AdminEmployeeCreateUser',
          component: () => import('@/views/Admin/Employee/AdminEmployeeCreateUser.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'employees/edit-user/:id',
          name: 'AdminEmployeeEditUser',
          component: () => import('@/views/Admin/Employee/AdminEmployeeEditUser.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'customers',
          name: 'AdminCustomerList',
          component: () => import('@/views/Admin/Customer/AdminCustomerList.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'customer/detail/:id',
          name: 'AdminCustomerDetail',
          component: () => import('@/views/Admin/Customer/AdminCustomerDetail.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'categories',
          name: 'AdminCategoryList',
          component: () => import('@/views/Admin/Motorbike/AdminCategoryList.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'category/create',
          name: 'AdminCategoryCreate',
          component: () => import('@/views/Admin/Motorbike/AdminCategoryCreate.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'category/:id',
          name: 'AdminCategoryDetail',
          component: () => import('@/views/Admin/Motorbike/AdminCategoryDetail.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'discounts',
          name: 'AdminDiscountList',
          component: () => import('@/views/Admin/Discount/AdminDiscountList.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'discount/create',
          name: 'AdminDiscountCreate',
          component: () => import('@/views/Admin/Discount/AdminDiscountCreate.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'discount/:id',
          name: 'AdminDiscountDetail',
          component: () => import('@/views/Admin/Discount/AdminDiscountDetail.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'statistics',
          name: 'AdminStatistics',
          component: () => import('@/views/Admin/Statistics/AdminStatistics.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'contracts',
          name: 'AdminContractList',
          component: () => import('@/views/Admin/Contract/AdminContractList.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'contract/detail/:id',
          name: 'AdminContractDetail',
          component: () => import('@/views/Admin/Contract/AdminContractDetail.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'incident/:contractId',
          name: 'AdminIncidentList',
          component: () => import('@/views/Admin/Incident/AdminIncidentList.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'payment/:id',
          name: 'AdminPaymentDetail',
          component: () => import('@/views/Admin/Contract/PaymentDetail.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        }
      ],
    },
    {
      path: '/Receptionist',
      children: [
        {
          path: 'customers',
          name: 'ReceptionistListCustomer',
          component: () => import('@/views/Receptionist/Customer/ReceptionistListCustomer.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'customer/create',
          name: 'ReceptionistCreateCustomer',
          component: () => import('@/views/Receptionist/Customer/ReceptionistCreateCustomer.vue'),
        },
        {
          path: 'motorbikes',
          name: 'ReceptionistMotorbikeList',
          component: () => import('@/views/Receptionist/Motorbikes/ReceptionistMotorbikeList.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'motorbike/detail/:id',
          name: 'ReceptionistMotorbikeDetail',
          component: () => import('@/views/Receptionist/Motorbikes/DetailMotorbike.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'contracts',
          name: 'ReceptionistListContract',
          component: () => import('@/views/Receptionist/Contract/ReceptionistListContract.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'contract/create/:customerId?',
          name: 'ReceptionistCreateContract',
          component: () => import('@/views/Receptionist/Contract/ReceptionistCreateContract.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'contract/select-motorbike',
          name: 'ReceptionistSelectMotorbike',
          component: () => import('@/views/Receptionist/Contract/ReceptionistSelectMotorbike.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'contract/detail/:id',
          name: 'ReceptionistContractDetail',
          component: () => import('@/views/Receptionist/Contract/ReceptionistContractDetail.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'contract/settlement/:id',
          name: 'ReceptionistSettlementContract',
          component: () =>
            import('@/views/Receptionist/Contract/ReceptionistSettlementContract.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'incident/create/:contractId?',
          name: 'ReceptionistCreateIncident',
          component: () => import('@/views/Receptionist/Incident/ReceptionistCreateIncident.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'payment/process/:contractId?',
          name: 'ReceptionistProcessPayment',
          component: () => import('@/views/Receptionist/Payment/ReceptionistProcessPayment.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'payment/:id',
          name: 'ReceptionistPaymentDetail',
          component: () => import('@/views/Receptionist/Payment/ReceptionistPaymentDetail.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
        {
          path: 'incident/:contractId',
          name: 'ReceptionistIncidentList',
          component: () => import('@/views/Receptionist/Incident/ReceptionistDetailIncident.vue'),
          meta: { requiresAuth: true, roles: ['Receptionist'] },
        },
      ],
    },
    {
      path: '/Maintenance',
      children: [
        {
          path: 'history',
          name: 'MaintenanceHistory',
          component: () => import('@/views/Maintenance/MaintenanceRecord/MaintenanceMaintenanceHistory.vue'),
          meta: { requiresAuth: true, roles: ['Maintenance'] },
        },
        {
          path: 'motorbikes',
          name: 'MaintenanceMotorbikeList',
          component: () => import('@/views/Maintenance/MaintenanceRecord/MaintenanceMotorbikeList.vue'),
          meta: { requiresAuth: true, roles: ['Maintenance'] },
        },
        {
          path: 'createMaintenanceRecord/:motorbikeId',
          name: 'MaintenanceCreateRecord',
          component: () => import('@/views/Maintenance/MaintenanceRecord/MaintenanceCreate.vue'),
          meta: { requiresAuth: true, roles: ['Maintenance'] },
        },
        {
          path: 'repairs',
          name: 'MaintenanceRepairs',
          component: () => import('@/views/Maintenance/MaintenanceRecord/MaintenanceRepairs.vue'),
          meta: { requiresAuth: true, roles: ['Maintenance'] },
        },
        {
          path: 'repairs/complete/:motorbikeId',
          name: 'MaintenanceCompleteRepairs',
          component: () => import('@/views/Maintenance/MaintenanceRecord/MaintenanceCompleteRepairs.vue'),
          meta: { requiresAuth: true, roles: ['Maintenance'] },
        },
        {
          path: 'incidents',
          name: 'MaintenanceIncidentList',
          component: () => import('@/views/Maintenance/Incident/MaintenanceIncidentList.vue'),
          meta: { requiresAuth: true, roles: ['Maintenance'] },
        },
        {
          path: 'incident/complete/:incidentId',
          name: 'MaintenanceIncidentComplete',
          component: () => import('@/views/Maintenance/Incident/MaintenanceIncidentComplete.vue'),
          meta: { requiresAuth: true, roles: ['Maintenance'] },
        }
      ],
    },
    {
      path: '/forbidden',
      name: 'Forbidden',
      component: () => import('@/views/Forbidden.vue'),
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'NotFound',
      component: () => import('@/views/NotFound.vue'),
    },
  ],
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth)
  const requiredRoles = to.matched.flatMap((record) => record.meta?.roles || [])

  let user = null
  if (token) {
    try {
      user = jwtDecode(token)
    } catch {
      localStorage.removeItem('token')
      return next({ name: 'Login' })
    }
  }

  if (requiresAuth && !token) {
    return next({ name: 'Login' })
  }

  if (requiredRoles.length > 0) {
    if (user && requiredRoles.includes(user.role)) {
      return next()
    } else {
      return next({ name: 'Forbidden' })
    }
  }

  next()
})

export default router
