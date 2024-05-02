library ieee;
use ieee.std_logic_1164.all, ieee.numeric_std.all;

use work.gumnut_defs.all;
use std.textio.all;

architecture struct of gumnut_with_mem is

  -- Instruction memory bus
  signal inst_cyc_o : std_logic;
  signal inst_stb_o : std_logic;
  signal inst_ack_i : std_logic;
  signal inst_adr_o : unsigned(11 downto 0);
  signal inst_dat_i : std_logic_vector(17 downto 0);
  -- Data memory bus
  signal data_cyc_o : std_logic;
  signal data_stb_o : std_logic;
  signal data_we_o : std_logic;
  signal data_ack_i : std_logic;
  signal data_adr_o : unsigned(7 downto 0);
  signal data_dat_o : std_logic_vector(7 downto 0);
  signal data_dat_i : std_logic_vector(7 downto 0);

  component gumnut is
    generic ( debug : boolean );
    port ( clk_i : in std_logic;
           rst_i : in std_logic;
           -- Instruction memory bus
           inst_cyc_o : out std_logic;
           inst_stb_o : out std_logic;
           inst_ack_i : in std_logic;
           inst_adr_o : out unsigned(11 downto 0);
           inst_dat_i : in std_logic_vector(17 downto 0);
           -- Data memory bus
           data_cyc_o : out std_logic;
           data_stb_o : out std_logic;
           data_we_o : out std_logic;
           data_ack_i : in std_logic;
           data_adr_o : out unsigned(7 downto 0);
           data_dat_o : out std_logic_vector(7 downto 0);
           data_dat_i : in std_logic_vector(7 downto 0);
           -- I/O port bus
           port_cyc_o : out std_logic;
           port_stb_o : out std_logic;
           port_we_o : out std_logic;
           port_ack_i : in std_logic;
           port_adr_o : out unsigned(7 downto 0);
           port_dat_o : out std_logic_vector(7 downto 0);
           port_dat_i : in std_logic_vector(7 downto 0);
           -- Interrupts
           int_req : in std_logic;
           int_ack : out std_logic );
  end component gumnut;

  component inst_mem is
    --generic ( IMem_file_name : string );
    port ( clk_i : in std_logic;
           cyc_i : in std_logic;
           stb_i : in std_logic;
           ack_o : out std_logic;
           adr_i : in unsigned(11 downto 0);
           dat_o : out std_logic_vector(17 downto 0) );
  end component inst_mem;

  component data_mem is
    --generic ( DMem_file_name : string );
    port ( clk_i : in std_logic;
           cyc_i : in std_logic;
           stb_i : in std_logic;
           we_i : in std_logic;
           ack_o : out std_logic;
           adr_i : in unsigned(7 downto 0);
           dat_i : in std_logic_vector(7 downto 0);
           dat_o : out std_logic_vector(7 downto 0) );
  end component data_mem;

begin

  core : component gumnut
    generic map ( debug => debug )
    port map ( clk_i      => clk_i,
               rst_i      => rst_i,
               inst_cyc_o => inst_cyc_o,
               inst_stb_o => inst_stb_o,
               inst_ack_i => inst_ack_i,
               inst_adr_o => inst_adr_o,
               inst_dat_i => inst_dat_i,
               data_cyc_o => data_cyc_o,
               data_stb_o => data_stb_o,
               data_we_o  => data_we_o,
               data_ack_i => data_ack_i,
               data_adr_o => data_adr_o,
               data_dat_o => data_dat_o,
               data_dat_i => data_dat_i,
               port_cyc_o => port_cyc_o,
               port_stb_o => port_stb_o,
               port_we_o  => port_we_o,
               port_ack_i => port_ack_i,
               port_adr_o => port_adr_o,
               port_dat_o => port_dat_o,
               port_dat_i => port_dat_i,
               int_req    => int_req,
               int_ack    => int_ack );

  core_inst_mem : component inst_mem
    --generic map ( IMem_file_name => IMem_file_name )
    port map ( clk_i => clk_i,
               cyc_i => inst_cyc_o,
               stb_i => inst_stb_o,
               ack_o => inst_ack_i,
               adr_i => inst_adr_o,
               dat_o => inst_dat_i );

  core_data_mem : component data_mem
    --generic map ( DMem_file_name => DMem_file_name )
    port map ( clk_i => clk_i,
               cyc_i => data_cyc_o,
               stb_i => data_stb_o,
               we_i  => data_we_o,
               ack_o => data_ack_i,
               adr_i => data_adr_o,
               dat_i => data_dat_o,
               dat_o => data_dat_i );

end architecture struct;
