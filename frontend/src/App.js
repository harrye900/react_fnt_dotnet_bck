import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';

const API_BASE = 'http://localhost:5000/api/gateway';

function App() {
  const [user, setUser] = useState(null);
  const [products, setProducts] = useState([]);
  const [cart, setCart] = useState([]);
  const [showCheckout, setShowCheckout] = useState(false);

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      const response = await axios.get(`${API_BASE}/products`);
      setProducts(JSON.parse(response.data));
    } catch (error) {
      console.error('Error fetching products:', error);
    }
  };

  const login = async () => {
    try {
      const response = await axios.post(`${API_BASE}/auth/login`, {
        email: 'user@test.com',
        password: 'password'
      });
      const userData = JSON.parse(response.data);
      setUser(userData);
    } catch (error) {
      console.error('Login failed:', error);
    }
  };

  const addToCart = async (product) => {
    if (!user) {
      alert('Please login first');
      return;
    }
    
    try {
      await axios.post(`${API_BASE}/cart/add`, {
        userId: user.userId,
        productId: product.id,
        quantity: 1,
        price: product.price
      });
      
      const existingItem = cart.find(item => item.id === product.id);
      if (existingItem) {
        setCart(cart.map(item => 
          item.id === product.id 
            ? { ...item, quantity: item.quantity + 1 }
            : item
        ));
      } else {
        setCart([...cart, { ...product, quantity: 1 }]);
      }
    } catch (error) {
      console.error('Error adding to cart:', error);
    }
  };

  const checkout = async () => {
    if (cart.length === 0) return;
    
    const total = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    
    try {
      const orderData = {
        userId: user.userId,
        items: cart.map(item => ({
          productId: item.id,
          quantity: item.quantity,
          price: item.price
        })),
        total,
        paymentDetails: {
          amount: total,
          cardNumber: '1234567890123456',
          expiryDate: '12/25',
          cvv: '123',
          userId: user.userId
        }
      };

      await axios.post(`${API_BASE}/orders/create`, orderData);
      alert('Order placed successfully!');
      setCart([]);
      setShowCheckout(false);
    } catch (error) {
      console.error('Checkout failed:', error);
      alert('Checkout failed');
    }
  };

  return (
    <div className="App">
      <header>
        <h1>Microservices E-Commerce</h1>
        {!user ? (
          <button onClick={login}>Login</button>
        ) : (
          <div>
            <span>Welcome, {user.email}</span>
            <button onClick={() => setShowCheckout(!showCheckout)}>
              Cart ({cart.length})
            </button>
          </div>
        )}
      </header>

      {showCheckout && (
        <div className="checkout">
          <h2>Shopping Cart</h2>
          {cart.map(item => (
            <div key={item.id} className="cart-item">
              {item.name} - ${item.price} x {item.quantity}
            </div>
          ))}
          <div className="total">
            Total: ${cart.reduce((sum, item) => sum + (item.price * item.quantity), 0).toFixed(2)}
          </div>
          <button onClick={checkout}>Place Order</button>
        </div>
      )}

      <div className="products">
        <h2>Products</h2>
        <div className="product-grid">
          {products.map(product => (
            <div key={product.id} className="product">
              <h3>{product.name}</h3>
              <p>${product.price}</p>
              <p>Stock: {product.stock}</p>
              <button onClick={() => addToCart(product)}>Add to Cart</button>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}

export default App;