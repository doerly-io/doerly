INSERT INTO auth."role" (
	id,
	name,
	date_created,
	last_modified_date
)
VALUES (
	1,
	'ADMIN',
	NOW(), 
	NOW()
);

INSERT INTO auth."user" (
	id,
	email, 
	password_hash, 
	password_salt, 
	is_email_verified,
	role_id,
	date_created, 
	last_modified_date
)
VALUES (
	1,
	'admin@admin.com', 
	'1tIAL92SG5Ia0rjumJGzSQFutG1knzwj0IXqKrl55342nqgBIbuVvr4cmZl2hoFIil6bdtgS/JHKVU91m03y9g==', 
	'r8RE93Pm6WnjKQxYx1t8GBduTFF0Ct299jjTBWXmaC41slvn5OpmxBlMAYP+uzSRuI3k/P/ANppp1z+2May663yMZRzC9QyoSvJN/gYDBpmoAxs1ihA8oXHWb6ozeh1HQHVWbW7vx+z9H3AIxlIdi5k7sZLiBdWuCAgTPEpO6fs=',
	true,
	1,
	NOW(), 
	NOW()
);

INSERT INTO profile."profile" (
	id, first_name, last_name, sex, user_id, date_created, last_modified_date
)
VALUES (
	1, 'Admin', 'Adminchenko', 1, 1, NOW(), NOW()
);
