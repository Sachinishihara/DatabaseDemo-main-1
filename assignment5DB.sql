INSERT INTO carriers (
    carrier_name,
    contact_url,
    contact_phone
)
VALUES
    ('DHL', 'https://www.dhl.com', '+49 228 767 676'),
    ('UPS', 'https://www.ups.com', '+1 800 742 5877');
    
    SELECT * FROM carriers;
    SELECT * FROM orders;


UPDATE orders
SET CarrierId      = 1,
    tracking_number = 'DH123456789',
    shipped_date    = NOW(),
    order_status    = 'Shipped'
WHERE order_id = 1; 

SELECT o.order_id,
       o.order_status,
       c.carrier_name,
       o.tracking_number,
       o.shipped_date,
       o.delivered_date
FROM orders o
LEFT JOIN carriers c ON o.CarrierId = c.CarrierId
WHERE o.order_id = 1;
