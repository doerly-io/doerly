INSERT INTO catalog.category (name, description, parent_id, is_deleted, is_enabled, date_created, last_modified_date)
VALUES 
-- Побутові послуги
(N'Побутові послуги', N'Усі послуги для дому та побуту', NULL, false, true, NOW(), NOW()),
(N'Прибирання', N'Квартири, офіси, після ремонту тощо', 1, false, true, NOW(), NOW()),
(N'Ремонт техніки', N'Ремонт пральних машин, холодильників та іншої техніки', 1, false, true, NOW(), NOW()),
(N'Вантажники та перевезення', N'Допомога з переїздом, вантажні перевезення', 1, false, true, NOW(), NOW()),
(N'Ремонт і будівництво', N'Ремонтні та будівельні роботи', 1, false, true, NOW(), NOW()),

-- IT та цифрові послуги
(N'IT та цифрові послуги', N'IT-фахівці, програмування, дизайн', NULL, false, true, NOW(), NOW()),
(N'Розробка сайтів', N'Frontend, backend, повна розробка', 6, false, true, NOW(), NOW()),
(N'Дизайн та поліграфія', N'UI/UX, логотипи, банери, презентації', 6, false, true, NOW(), NOW()),
(N'SMM та маркетинг', N'Соціальні мережі, реклама, просування', 6, false, true, NOW(), NOW()),
(N'Технічна підтримка', N'Налаштування серверів, ПЗ, супровід', 6, false, true, NOW(), NOW()),

-- Освіта
(N'Освіта', N'Навчальні послуги', NULL, false, true, NOW(), NOW()),
(N'Репетиторство', N'Шкільні предмети, мови, підготовка до ЗНО', 11, false, true, NOW(), NOW()),
(N'Онлайн-курси', N'Вебінари, курси, підвищення кваліфікації', 11, false, true, NOW(), NOW()),

-- Догляд та краса
(N'Краса і здоров’я', N'Косметологія, масаж, тренери', NULL, false, true, NOW(), NOW()),
(N'Косметологія', N'Макіяж, депіляція, догляд за шкірою', 14, false, true, NOW(), NOW()),
(N'Фітнес та спорт', N'Персональні тренери, тренування', 14, false, true, NOW(), NOW()),

-- Інше
(N'Інші послуги', N'Інші категорії, які не входять до основних', NULL, false, true, NOW(), NOW());
