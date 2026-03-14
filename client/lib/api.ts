   // Fetches from your .NET backend at e.g. http://localhost:5088/api/data/order?orderId=...
    export async function getOrderDetails(orderId: number) {
     const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;

     const response = await fetch(`${API_BASE}/api/data/order?orderId=${orderId}`);
       if (!response.ok) {
         console.error('Failed to fetch order details');
         throw new Error('Failed to fetch order details');
       }
       console.log('Order details fetched successfully');
       return response.json();
     }