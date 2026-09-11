# CashFlow

Este projeto acadêmico foi desenvolvido com finalidade exclusivamente educacional, tendo como objetivo a aplicação prática de conceitos, tecnologias e boas práticas de desenvolvimento de software.

## Sobre o Projeto

Esta API, desenvolvida utilizando **.NET 10**, adota os princípios do **Domain-Driven Design (DDD)** para oferecer uma solução estruturada e eficaz no gerenciamento de despesas pessoais. O principal objetivo é permitir 
que os usuários registrem suas despesas, detalhando informações como título, data, hora, descrição, valor e tipo de pagamento, com os dados sendo armazenados de forma segura em um banco de dados Postgre.

A arquitetura da API baseia-se em REST, utilizando métodos HTTP padrão para uma comunicação eficiente e simplificada. Além disso, é complementada por uma documentação **Scalar utilizando OpenAPI**, que proporciona uma 
interface gráfica interativa para que os desenvolvedores possam explorar e testar os endpoints de maneira fácil.

Dentre os pacotes NuGet utilizados, o **AutoMapper** é o responsável pelo mapeamento entre objetos de domínio e requisição/resposta, reduzindo a necessidade de código repetitivo e manual. O **FluentAssertions** é utilizado 
para testes de unidade para tornar as verificações mais legíveis, ajudando a escrever testes claros e compreensíveis. Para as validações, o **FluentValidation** é usado para implementar regras de validações de forma 
simples e intuitiva nas classes de requisições, mantendo o código limpo e fácil de manter. Por fim, o **EntityFramework** atua como um ORM (Object-Relational Mapper) que simplifica as interações com o banco de dados, 
permitindo o uso de consultas SQL.
