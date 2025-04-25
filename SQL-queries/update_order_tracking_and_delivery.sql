-- Update order with shipping info
UPDATE orders
SET "CarrierId"      = 1,
    tracking_number = 'DH123456789',
    shipped_date    = NOW(),
    order_status    = 'Shipped'
WHERE order_id = 1;

-- To view updated shipping info
SELECT o.order_id,
       o.order_status,
       c.carrier_name,
       o.tracking_number,
       o.shipped_date,
       o.delivered_date
FROM orders o
LEFT JOIN carriers c ON o."CarrierId" = c.carrier_id 
WHERE o.order_id = 1;

-- Update order to mark as delivered
UPDATE orders
SET delivered_date = NOW(),
    order_status   = 'Delivered'
WHERE order_id = 1;

-- View updated delivery info
SELECT o.order_id,
       o.order_status,
       c.carrier_name,
       o.tracking_number,
       o.shipped_date,
       o.delivered_date
FROM orders o
LEFT JOIN carriers c ON o."CarrierId" = c.carrier_id 
WHERE o.order_id = 1;
