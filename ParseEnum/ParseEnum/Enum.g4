grammar Enum;

prog: usingDirective;

usingDirective: (keyword ID ('.' ID)* ';' stat*)* stat* namespaceName;

namespaceName 
	: keyword ID ('.' ID)* stat* '{' stat? enumDeclaration stat* '}'
	;

enumDeclaration: keyword? 'enum' ID NEWLINE '{' statementOrAnnotation* '}';

statementOrAnnotation: stat | dataAnnotation;

stat: NEWLINE
    | assign ','? NEWLINE?
    ;

dataAnnotation: '[' (ID | '(' | ')' | '=' | ';' | ',' | '"')* ']' NEWLINE?;

assign: ID ASSIGNOPERATOR expr;

expr: INT
    | ID
    | '"' expr '"'
    ;

keyword: PUBLIC
		 |USING
		 |NAMESPACE
		 ;

SINGLE_LINE_DOC_COMMENT: '///' InputCharacter*    -> skip;
DELIMITED_DOC_COMMENT:   '/**' .*? '*/'           -> skip;
SINGLE_LINE_COMMENT:     '//'  InputCharacter*    -> skip;
DELIMITED_COMMENT:       '/*'  .*? '*/'           -> skip;

fragment InputCharacter:       ~[\r\n\u0085\u2028\u2029];

PUBLIC: 'public';
USING: 'using';
NAMESPACE:     'namespace';

ASSIGNOPERATOR: '=' ;
ID  :   [a-zA-Z0-9]+ ;   
INT :   [0-9]+ ;    
NEWLINE:'\r'? '\n' ;     
WS  :   [ \t]+ -> skip ; 
