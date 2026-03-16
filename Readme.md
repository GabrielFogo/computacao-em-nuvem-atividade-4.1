### Gabriel Costal Fogo - Computação em numvem 2 - 6° Semestre

### Descrição

O projeto é uma aplicação que permite cadastrar e listar seus produtos e gerenciar suas imagens na nuvem

Escolhi plataforma do google cloud por já ter integrado com azure algumas vezes. Os serviçoes ultilizados foram:
- Cloud Storage: criei o bucket para hospedar as imagens
- Cloud Sql: criei o banco de dados mysql

### Fluxo do projeto

flowchart TD

    A[Início]

    A --> B[Criar produtos]
    B --> B1[s27]
    B1 -->|"s28"| B2[Produtos criados no banco]
    B1 -->|"s29"| B3[Endpoint POST /produtos]
    B3 --> B4[ProdutoService cria produto]
    B4 --> B2

    B2 --> C[Listar produtos]
    C --> C1[Endpoint GET /produtos]
    C1 --> C2[ProdutoService busca no banco]

    C2 --> D[Adicionar imagem ao produto]
    D --> D1["Endpoint POST /produtos/{id}/imagens"]
    D1 --> D2[ProdutoService.InserirImagem]

    D2 --> D3[Chama IStorageService interface]
    D3 --> D4[Implementação GoogleCloudStorageService]
    D4 --> D5[Google Cloud Storage salva arquivo]

    D5 --> D6[Salvar caminho da imagem no banco]

    D6 --> E[Listar imagens de um produto]
    E --> E1["Endpoint GET /produtos/{id}/imagens"]
    E1 --> E2[ProdutoService consulta imagens no banco]
    E2 --> E3[Montar URL usando StorageBaseUrl]

    E3 --> F[Apagar imagem]
    F --> F1["Endpoint DELETE /imagens/{id}"]
    F1 --> F2[ProdutoService.RemoverImagem]

    F2 --> F3[Chama IStorageService interface]
    F3 --> F4[GoogleCloudStorageService remove arquivo no Google Cloud]

    F4 --> F5[Remove registro da imagem no banco]

    F5 --> G[Fim]

### Configuração dos serviços

#### Bucket
1. Criei uma conta de serviço para gerenciar os buckets e criei o bucket no google cloud
2. Tentei criar gerar as crendencias mas a conta não permitiu, achei a solução de logar no google cloud usando o seguinte comando:
```
gcloud auth application-default login
```
3. Instalei o sdk do google cloud para usar buckets no .net

#### Banco de dados

1. Criei uma nova instancia de banco de dados no cloud sql
2. Criei um novo banco de dados para aplicação
3. As migrations do .net criaram as tabelas e os 5 produtos

### Como rodar

#### Rodando com .net pelo visual studio
Nescessário ter a versão 10 do .Net
1. Configure o appsettings.json ou o user secrets
2. Rodar o projeto no visual studio

### Rodando via compose
1. Configurar as credencias do google cloud no arquivo docker-compose
2. Configurar a connection string se for usar o banco da nuvem
3. Rodar o comando:
```
docker compose up -d --build
```