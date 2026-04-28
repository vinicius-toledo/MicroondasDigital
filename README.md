# 🍿 MicroondasDigital

Simulador de micro-ondas digital desenvolvido em C# com ASP.NET MVC e Web API, com autenticação JWT e separação de camadas.

---

## 📋 Pré-requisitos

- Visual Studio 2019 ou superior
- .NET Framework 4.7.2
- IIS Express (já vem com o Visual Studio)

---

## 🚀 Como executar o projeto

O projeto possui **dois aplicativos** que precisam rodar ao mesmo tempo:

- `MicroondasDigital` → interface web (MVC)
- `MicroondasDigital.API` → API REST com a lógica de negócio

### Passo 1 — Configurar múltiplos projetos de inicialização

1. Clique com botão direito na **Solução** no Gerenciador de Soluções
2. Vá em **Propriedades**
3. Em **Projeto de Inicialização**, selecione **Vários projetos de inicialização**
4. Defina os dois projetos como **Iniciar**:
   - `MicroondasDigital` → Iniciar
   - `MicroondasDigital.API` → Iniciar
5. Clique em **OK**

### Passo 2 — Executar

Pressione **F5** ou clique em **Iniciar**. Dois navegadores vão abrir — um para cada projeto.

---

## 🔐 Login

Ao abrir o sistema, você será redirecionado para a tela de **Configurações**, onde deve informar:

| Campo | Valor |
|---|---|
| Usuário | `admin` |
| Senha | `admin123` |
| URL da API | URL que aparecer na barra de endereço do navegador da API |

> A porta da API aparece automaticamente na barra de endereço quando o projeto inicia. Copie essa URL e cole no campo **URL da API**.

Após preencher, clique em **Conectar**. O canto superior direito mostrará **API: Conectado ✓** em verde.

---

## 🧪 Como testar as funcionalidades

### ✅ Aquecimento simples

1. No campo **Tempo**, digite um valor entre `1` e `120` segundos
2. No campo **Potência**, digite um valor entre `1` e `10`
3. Clique em **Iniciar Aquecimento / +30s**
4. O resultado aparece abaixo com uma sequência de caracteres representando os pulsos de energia

**Casos de teste:**
- Deixar o tempo vazio → usa 30 segundos por padrão
- Deixar a potência vazia → usa potência 10 por padrão
- Digitar tempo acima de `120` → exibe mensagem de erro
- Digitar potência acima de `10` → exibe mensagem de erro

---

### ⏸️ Pausar e Cancelar

- Clique em **Pausar / Cancelar** durante um aquecimento ativo
- **Primeira vez:** pausa o aquecimento (mantém o tempo restante na tela)
- **Segunda vez:** cancela e limpa a tela

---

### ➕ Acréscimo de 30 segundos

1. Inicie um aquecimento normalmente
2. Sem pausar, clique novamente em **Iniciar Aquecimento / +30s**
3. O sistema acrescenta **+30 segundos** ao tempo atual

> **Atenção:** o tempo total não pode ultrapassar 120 segundos. Se ultrapassar, exibe erro.

---

### 🍕 Programas pré-definidos

Clique em qualquer botão de programa rápido na parte superior:

| Programa | Caractere | Tempo | Potência |
|---|---|---|---|
| Pipoca | `!` | 3 min | 7 |
| Leite | `@` | 5 min | 5 |
| Carne de boi | `#` | 14 min | 4 |
| Frango | `$` | 8 min | 7 |
| Feijão | `%` | 8 min | 9 |

> **Atenção:** ao usar um programa pré-definido, o acréscimo de +30s **não é permitido**.

---

### ✏️ Cadastrar programa customizado

1. Preencha o formulário **Cadastrar Novo Programa** na parte inferior da tela
2. Clique em **Salvar Receita**
3. O novo programa aparece como botão na lista e pode ser usado normalmente

**Casos de teste:**
- Usar o caractere `.` → erro (reservado para aquecimento padrão)
- Usar um caractere já cadastrado (ex: `!` da Pipoca) → erro de duplicidade
- Preencher tudo corretamente → mensagem de sucesso e botão aparece na lista

---

## 🔑 Credenciais

| Campo | Valor |
|---|---|
| Usuário | `admin` |
| Senha | `admin123` |

> O token JWT gerado expira em **2 horas**.
