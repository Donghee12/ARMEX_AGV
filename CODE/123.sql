-- ⚠ 외래키 제약 비활성화
SET FOREIGN_KEY_CHECKS = 0;

-- ✅ 데이터베이스 생성 및 선택
CREATE DATABASE IF NOT EXISTS factory_db;
USE factory_db;

-- ✅ 기존 테이블 제거 (외래키 순서 고려)
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS order_items;
DROP TABLE IF EXISTS inventory;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS admin_users;
DROP TABLE IF EXISTS inventory;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS emergency_logs;
DROP TABLE IF EXISTS vehicle_orders;

-- ⚠ 외래키 제약 다시 활성화
SET FOREIGN_KEY_CHECKS = 1;

-- ✅ 관리자/직원 테이블
CREATE TABLE admin_users (
    admin_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    user_type VARCHAR(20) DEFAULT 'staff'  -- 'admin' 또는 'staff'
);


-- ✅ 제품 테이블 (✔ code_name 컬럼 추가됨)
CREATE TABLE products (
    product_id INT AUTO_INCREMENT PRIMARY KEY,
    product_name VARCHAR(100) NOT NULL,
    code_name VARCHAR(100) NOT NULL UNIQUE
);


-- ✅ 재고 테이블
CREATE TABLE inventory (
    inventory_id INT AUTO_INCREMENT PRIMARY KEY,
    product_id INT NOT NULL,
    stock INT DEFAULT 0,
    status VARCHAR(50) DEFAULT '정상',
    FOREIGN KEY (product_id) REFERENCES products(product_id)
);


-- ✅ 긴급 정지 테이블
CREATE TABLE emergency_logs (
    log_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50),
    event_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    description TEXT,
    status VARCHAR(50) DEFAULT '대기중' ,
    image_path VARCHAR(255)
);

ALTER table emergency_logs
add	 constraint fk_emergency_username
foreign key (username) references admin_users(username);

-- ✅ 주문 테이블
CREATE TABLE orders (
    order_id INT AUTO_INCREMENT PRIMARY KEY,
    company_name VARCHAR(100),
    order_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(50) DEFAULT '대기중'
);


CREATE TABLE order_items (
    order_item_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT,
    product_id INT,
    quantity INT,
	Pquantity INT,
    FOREIGN KEY (order_id) REFERENCES orders(order_id),
    FOREIGN KEY (product_id) REFERENCES products(product_id)
);




-- ✅ 긴급정비버튼활성화/비활성화
CREATE TABLE system_status (
    id INT PRIMARY KEY DEFAULT 1,
    is_emergency BOOLEAN DEFAULT FALSE
);

INSERT INTO system_status (id, is_emergency) VALUES (1, FALSE); -- 초기 상태는 정상



-- ✅ 초기 관리자/직원 계정 추가
INSERT INTO admin_users (username, password, user_type) VALUES
('admin', '1234', 'admin'),
('staff', '1234', 'staff'),
('admin1', 'admin1234', 'admin');


-- ✅ 초기 제품 데이터 (✔ code_name도 함께 입력)
INSERT INTO products (product_id, product_name, code_name) VALUES
(1, '의자', 'chair'),
(2, '캐비넷', 'cabinet'),
(3, '책상', 'table');

-- ✅ 초기 재고 데이터
INSERT INTO inventory (product_id, stock) VALUES
(1, 100),
(2, 50),
(3, 30);

-- ✅ 초기 주문 데이터
-- 주문 1: 삼성전자
INSERT INTO orders (order_id, company_name) VALUES (1, '삼성전자');
INSERT INTO order_items (order_id, product_id, quantity, Pquantity) VALUES
(1, 1, 7, 0),  -- chair
(1, 2, 4, 0);  -- cabinet


-- 주문 2: 어딘가
INSERT INTO orders (order_id, company_name) VALUES (2, '어딘가');
INSERT INTO order_items (order_id, product_id, quantity,Pquantity) VALUES
(2, 1, 8,0),  -- chair
(2, 2, 7,0),  -- cabinet
(2, 3, 4,0);  -- table


-- 긴급 로그
INSERT INTO emergency_logs (username, description, status) VALUES
('admin', '라인1 비상 정지 발생', '대기중'),
('staff', '창고 화재 감지', '처리중');


-- 기록 지워지는지 테스트용 로그
INSERT INTO emergency_logs (username, event_time, description, status) VALUES
('admin', '2023-01-15 10:00:00', '라인1 비상 정지 발생', '처리됨'),
('staff', '2024-06-10 09:30:00', '배터리 폭발 감지', '대기중'),
('admin1', '2022-03-25 14:15:00', '로봇팔 장애 발생', '처리됨'),
('staff', '2023-08-05 16:45:00', '화재 경보 발생', '처리됨'),
('admin', '2021-11-20 08:10:00', '센서 작동 이상 감지', '대기중');

-- Safe update mode 해제
SET SQL_SAFE_UPDATES = 0;


-- 작업 후 다시 안전 모드 켜기 (선택 사항)
SET SQL_SAFE_UPDATES = 1;



-- 제품 ID 참고: 1 = chair, 2 = cabinet, 3 = table



-- 5년치 주문 데이터 (2020~2024, 다양한 회사명 포함)
INSERT INTO orders (order_id, company_name, order_time, status) VALUES
(1, '삼성전자', '2020-03-15 10:00:00', '처리됨'),
(2, 'LG전자', '2020-07-22 11:20:00', '처리됨'),
(3, '현대자동차', '2021-01-11 09:30:00', '처리됨'),
(4, '카카오', '2021-12-05 14:00:00', '처리됨'),
(5, '네이버', '2022-04-18 15:30:00', '처리됨'),
(6, '삼성전자', '2022-08-03 10:45:00', '처리됨'),
(7, 'LG전자', '2023-02-14 08:00:00', '처리됨'),
(8, '카카오', '2023-06-27 17:10:00', '처리됨'),
(9, '현대자동차', '2024-01-09 12:25:00', '처리됨'),
(10, '네이버', '2024-05-20 16:50:00', '처리됨'),
(11, '세진 컴퍼니', '2025-05-29 15:51:00', '대기중'),
(12, '세진 물류', '2025-05-29 15:52:00', '대기중'),
(13, '상현 물산', '2025-05-29 12:52:00', '대기중'),
(14,'YFS','2025-05-29 15:52:00','대기중'),
(15,'상현 금융','2025-06-02 15:52:00','대기중'),
(16,'동희 tech','2025-06-01 15:52:00','대기중'),
(17,'동희(주)','2025-06-02 14:52:00','대기중'),
(18,'요한 철강','2025-06-02 15:58:00','대기중'),
(19,'요한 전자','2025-06-02 15:14:00','대기중'),
(20,'주언 선박','2025-06-02 15:39:00','대기중'),
(21,'주언 통신','2025-06-03 15:14:00','대기중');
-- 각 주문에 대한 제품 처리 수량 기록
INSERT INTO order_items (order_id, product_id, quantity, Pquantity) VALUES
-- 2020
(1, 1, 10, 10),
(1, 2, 5, 5),
(2, 1, 3, 3),
(2, 3, 2, 2),

-- 2021
(3, 2, 6, 6),
(3, 3, 4, 4),
(4, 1, 7, 7),
(4, 3, 5, 5),

-- 2022
(5, 1, 6, 6),
(5, 2, 3, 3),
(6, 3, 4, 4),
(6, 1, 2, 2),

-- 2023
(7, 1, 8, 8),
(7, 2, 4, 4),
(8, 3, 5, 5),

-- 2024
(9, 2, 7, 7),
(9, 3, 6, 6),
(10, 1, 9, 9),

-- 2025
(11,1,2,0),
(12,1,1,0),
(12,2,1,0),

-- test dummy
(13,1,1,0),
(13,2,1,0),
(14,2,1,0),
(14,3,1,0),
(15,1,1,0),
(16,1,1,0),
(16,3,1,0),
(17,2,2,0),
(18,1,1,0),
(18,3,1,0),
(19,2,2,0),
(20,2,1,0),
(20,3,1,0),
(21,1,2,0);
