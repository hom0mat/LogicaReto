library ieee;
use ieee.std_logic_1164.all, ieee.numeric_std.all;

entity gumnut is
  generic ( debug : boolean := false );
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
end entity gumnut;
